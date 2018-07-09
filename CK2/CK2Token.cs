using System.Collections.Generic;
using System.Linq;

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

            var list = new List<CK2Token>();

            switch(Type) {

                case CK2TokenTypes.Object:

                    var fields = GetObject();

                    foreach(var field in fields) {
                        field.Value.Parent = this;
                        list.Add(field.Value);
                    }

                    break;

                case CK2TokenTypes.Value:
                    var newToken = new CK2Token(Value.ToString(), CK2TokenTypes.Value, Value);
                    newToken.Parent = this;
                    list.Add(newToken);
                    break;
            }

            Value = list;
            Type = CK2TokenTypes.List;
        }
        
        public CK2Token AddField(string token) {
            
            if(Type == CK2TokenTypes.List) {
                Add(token);
                return this;
            }

            var fields = GetObject();

            if(fields.ContainsKey(token)) {
                var field = fields[token];

                if(field.Type != CK2TokenTypes.List) {
                    field.ConvertToList();
                }
                
                return field;
            }
            
            fields.Add(token, new CK2Token(token) { Parent = this });

            return fields[token];
        }

        public CK2Token Add(string key) {
            var list = GetList();
            var item = list.FirstOrDefault();
            var type = CK2TokenTypes.Undefined;

            if(item != null) {
                type = item.Type;
            }

            var token = new CK2Token(key, type, key);
            token.Parent = this;

            GetList().Add(token);
            return token;
        }

        public void Add(CK2Object obj) {
            var token = new CK2Token("", CK2TokenTypes.Object, obj);
            token.Parent = this;

            GetList().Add(token);
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
