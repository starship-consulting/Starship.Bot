namespace Starship.Bot.CK2 {
    public class CK2Node {

        public CK2Node(CK2NodeTypes type) : this(type, string.Empty) {
        }

        public CK2Node(CK2NodeTypes type, object value) {
            Type = type;
            Value = value;
        }
        
        public CK2NodeTypes Type { get; set; }

        public object Value { get; set; }

        public override string ToString() {
            return "(" + Type + ") " + Value;
        }
    }
}