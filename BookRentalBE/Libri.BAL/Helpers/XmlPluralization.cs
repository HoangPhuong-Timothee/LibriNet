namespace Libri.BAL.Helpers
{
    public static class XmlPluralization
    {
        public static string Pluralize(string name)
        {
            if (name.EndsWith("y"))
            {
                return name.Substring(0, name.Length - 1) + "ies";
            }
            else if (name.EndsWith("s") || name.EndsWith("sh") || name.EndsWith("ch") || name.EndsWith("x") || name.EndsWith("z")) 
            { 
                return name + "es"; 
            } 
            else if (name.EndsWith("f")) 
            { 
                return name.Substring(0, name.Length - 1) + "ves"; 
            } 
            else if (name.EndsWith("fe")) 
            { 
                return name.Substring(0, name.Length - 2) + "ves"; 
            } 
            else 
            { 
                return name + "s"; 
            }
        }
    }
}
