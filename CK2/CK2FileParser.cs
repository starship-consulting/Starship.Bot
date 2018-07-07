using System.Linq;

namespace Starship.Bot.CK2 {
    public class CK2FileParser {

        public CK2FileParser() {
            Root = new CK2Object();
        }

        public void Read(string data) {

            Context = null;
            
            var lexer = new CK2FileLexer();
            var tokens = lexer.Read(data);
            
            foreach (var token in tokens) {
                
                var index = tokens.IndexOf(token);

                var previous = tokens.Skip(index - 10).Take(10).ToList();
                var next = tokens.Skip(index).Take(10).ToList();
                
                switch (token.Type) {

                    case CK2NodeTypes.Assignment:
                        Context.ConvertToValue();
                        break;

                    case CK2NodeTypes.LeftBrace: // Only if it's being assigned??  Consider lists of objects
                        Context.ConvertToObject();
                        break;

                    case CK2NodeTypes.RightBrace:
                        ExitContext();
                        break;

                    case CK2NodeTypes.NewLine:
                        break;

                    default:
                        
                        if(Context != null) {
                            if(Context.Type == CK2TokenTypes.Value) {
                                Context.Value = token.Value;
                                ExitContext();
                                break;
                            }

                            CloseContext();
                        }

                        EnterContext(token.Value.ToString());

                        break;
                }
            }
        }
        
        private void CloseContext() {
            if(Context != null && Context.Type == CK2TokenTypes.Undefined) {
                Context.ConvertToValue();

                if(Context.Value == null) {
                    Context.Value = Context.Key;
                }
                
                ExitContext();
                Context.ConvertToList();
            }
        }

        private void ExitContext() {

            todo:

            /*
            8 = {
			        add_building = ct_guard_5
			        add_building = ct_university_3
			        add_building = ct_training_grounds_6
		        }
            */


            /*
            8: {
	            add_building: [ct_guard_5, ct_university_3, ct_training_grounds_6]
            }
             */
            
            CloseContext();
            Context = Context.Parent;
        }

        private void SetRoot(string key, CK2Token value) {
            if(Root.ContainsKey(key)) {
                Root[key] = value;
            }
            else {
                Root.Add(key, value);
            }
        }

        private CK2Token GetRoot(string key) {
            if(!Root.ContainsKey(key)) {
                Root.Add(key, new CK2Token(key, CK2TokenTypes.Value, ""));
            }

            return Root[key];
        }

        private void EnterContext(string key) {

            if (Context == null) {

                var context = new CK2Token(key);
                context.Parent = Context;
                Context = context;

                SetRoot(Context.Key, Context);
            }
            else {
                if(Context.Type == CK2TokenTypes.List) {
                    Context.Add(key, CK2TokenTypes.Undefined);
                }
                else {
                    Context = Context.AddField(key);
                }
            }
        }
        
        public CK2Token Context { get; set; }

        public CK2Object Root { get; set; }
    }
}