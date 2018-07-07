using System.Collections.Generic;

namespace Starship.Bot.CK2
{
    public class CK2Token {

        public CK2Token(string key, CK2TokenTypes type = CK2TokenTypes.Undefined, object value = null) {
            Key = key;
            Type = type;
            Value = value;
        }

        public void ConvertToValue() {
            Type = CK2TokenTypes.Value;
            Value = null;
        }

        public void ConvertToObject() {
            Type = CK2TokenTypes.Object;
            Value = new CK2Object();
        }

        public void ConvertToList() {
            if(Type == CK2TokenTypes.List) {
                return;
            }

            var fields = GetObject();
            var value = new List<CK2Token>();

            foreach(var field in fields) {
                value.Add(field.Value);
            }

            Value = value;
            Type = CK2TokenTypes.List;
        }
        
        public CK2Token AddField(string token) {
            
            var fields = GetObject();
            var newToken = new CK2Token(token) { Parent = this };

            if(fields.ContainsKey(token)) {
                var field = fields[token];

                if(field.Type != CK2TokenTypes.List) {
                    fields.Remove(token);

                    var listToken = new CK2Token(token, CK2TokenTypes.List, new List<CK2Object> { field.GetObject() });
                    listToken.Parent = this;
                    fields.Add(token, listToken);
                }
                
                return fields[token];
            }

            GetObject().Add(token, newToken);

            return fields[token];
        }

        public void Add(string key, CK2TokenTypes type) {
            GetList().Add(new CK2Token(key, type, key));
        }

        public void Add(CK2Object obj) {
            GetList().Add(new CK2Token("", CK2TokenTypes.Object, obj));
        }

        public void Set(object value, CK2TokenTypes type) {
            Value = value;
            Type = type;
        }

        public List<CK2Token> GetList() {
            return Value as List<CK2Token>;
        }

        public CK2Object GetObject() {
            return Value as CK2Object;
        }
        
        public string Key { get; set; }

        public object Value { get; set; }
        
        public CK2TokenTypes Type { get; private set; }
        
        public CK2Token Parent { get; set; }

        public override string ToString() {
            return "(" + Type + ") " + Key + " = " + Value;
        }
    }
}
