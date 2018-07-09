using System;
using System.Collections.Generic;
using System.Linq;

namespace Starship.Bot.CK2 {

    public class CK2FileLexer {

        public CK2FileLexer() {
            Nodes = new List<CK2Node>();
            Token = string.Empty;
        }

        public List<CK2Node> Read(string data) {
            Nodes = new List<CK2Node>();
            var inQuotation = false;

            data = data.Replace("\n", Environment.NewLine);

            foreach (var line in data.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)) {
                var text = line.Replace("\t", "");
                
                foreach (var character in text) {

                    var previous = Nodes.LastOrDefault();
                    
                    if(character == '#') {
                        EndToken(CK2NodeTypes.NewLine);
                        break;
                    }

                    switch(character) {
                        case ' ':
                            if(!inQuotation) {
                                EndToken(CK2NodeTypes.Whitespace);
                            }
                            break;
                        case '"':
                            Token += character;
                            inQuotation = !inQuotation;
                            break;
                        case '=':
                            EndToken(CK2NodeTypes.Assignment);
                            break;
                        case '>':
                            EndToken(CK2NodeTypes.GreaterThan);
                            break;
                        case '<':
                            EndToken(CK2NodeTypes.LessThan);
                            break;
                        case '{':
                            EndToken(CK2NodeTypes.LeftBrace);
                            break;
                        case '}':
                            EndToken(CK2NodeTypes.RightBrace);
                            break;
                        default:
                            Token += character;
                            break;
                    }
                }

                EndToken(CK2NodeTypes.NewLine);
            }

            return Nodes;
        }

        private void EndToken(CK2NodeTypes type) {
            var trimmed = Token.Trim();
            Token = string.Empty;

            if(type == CK2NodeTypes.Assignment || type == CK2NodeTypes.GreaterThan || type == CK2NodeTypes.GreaterThanOrEqual || type == CK2NodeTypes.LessThan || type == CK2NodeTypes.LessThanOrEqual) {
                
                var previous = Nodes.LastOrDefault();

                if(previous != null) {

                    if(type == CK2NodeTypes.Assignment) {
                        if(previous.Type == CK2NodeTypes.GreaterThan) {
                            previous.Type = CK2NodeTypes.GreaterThanOrEqual;
                        }
                        else if(previous.Type == CK2NodeTypes.LessThan) {
                            previous.Type = CK2NodeTypes.LessThanOrEqual;
                        }
                    }

                    if(string.IsNullOrEmpty(trimmed)) {
                        if(previous.Type == CK2NodeTypes.Value) {
                            previous.Type = type;
                            previous.Value = previous.Value.ToString().Substring(1);
                        }

                        return;
                    }
                }
                
                Nodes.Add(new CK2Node(type, trimmed));
                return;
            }
            
            if(!string.IsNullOrEmpty(trimmed)) {

                if(trimmed.StartsWith("\"")) {
                    Nodes.Add(new CK2Node(CK2NodeTypes.Value, trimmed.Substring(1, trimmed.Length - 2)));
                }
                else {
                    float value;

                    if(float.TryParse(trimmed, out value)) {
                        if(trimmed.Contains(".")) {
                            Nodes.Add(new CK2Node(CK2NodeTypes.Value, value));
                        }
                        else {
                            Nodes.Add(new CK2Node(CK2NodeTypes.Value, int.Parse(trimmed)));
                        }
                    }
                    else {
                        if(type == CK2NodeTypes.Assignment) {
                            Nodes.Add(new CK2Node(CK2NodeTypes.Value, trimmed));
                        }
                        else {
                            if(trimmed == "yes") {
                                Nodes.Add(new CK2Node(CK2NodeTypes.Value, true));
                            }
                            else if(trimmed == "no") {
                                Nodes.Add(new CK2Node(CK2NodeTypes.Value, false));
                            }
                            else {
                                Nodes.Add(new CK2Node(CK2NodeTypes.Value, "@" + trimmed));
                            }
                        }
                    }
                }
            }
            
            if(type == CK2NodeTypes.Whitespace) {
                return;
            }

            Nodes.Add(new CK2Node(type));
        }

        private string Token { get; set; }

        private List<CK2Node> Nodes { get; set; }
    }
}