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
            var exitAfterAssignment = false;
            
            foreach (var token in tokens) {
                
                var index = tokens.IndexOf(token);
                var previousList = tokens.Skip(index - 10).Take(10).ToList();
                var nextList = tokens.Skip(index).Take(10).ToList();
                
                switch (token.Type) {

                    case CK2NodeTypes.GreaterThan:
                    case CK2NodeTypes.LessThan:
                    case CK2NodeTypes.GreaterThanOrEqual:
                    case CK2NodeTypes.LessThanOrEqual:
                    case CK2NodeTypes.Assignment:

                        /*if(Context != null && Context.Type == CK2TokenTypes.List) {
                            ExitContext();
                        }*/

                        exitAfterAssignment = true;
                        EnterContext(token.Value.ToString());
                        break;

                    case CK2NodeTypes.LeftBrace:
                        exitAfterAssignment = false;
                        Context.ConvertToObject();
                        break;

                    case CK2NodeTypes.RightBrace:
                        ExitContext();
                        break;

                    case CK2NodeTypes.NewLine:
                        break;

                    default:
                        
                        if(Context.Type == CK2TokenTypes.List) {
                            Context.Add(token.Value.ToString());

                            if(exitAfterAssignment) {
                                ExitContext();
                            }
                        }
                        else if(Context.Type == CK2TokenTypes.Object) {
                            Context.ConvertToList();
                            Context.Add(token.Value.ToString());
                        }
                        else {
                            Context.Set(token.Value, CK2TokenTypes.Value);
                            ExitContext();
                        }

                        exitAfterAssignment = false;
                        
                        break;
                }
            }
        }

        private void ExitContext() {
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
        
        private void EnterContext(string key) {

            if (Context == null) {
                Context = new CK2Token(key);
                SetRoot(Context.Key, Context);
            }
            else {
                Context = Context.AddField(key);
            }
        }
        
        public CK2Token Context { get; set; }

        public CK2Object Root { get; set; }
    }
}