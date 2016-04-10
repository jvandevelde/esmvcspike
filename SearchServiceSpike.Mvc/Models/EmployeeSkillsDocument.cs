using System.Collections.Generic;
using System.Text;
using Nest;

namespace WebApplication1.Models
{
    public class EmployeeSkillsDocument
    {
        public int Id { get; set; }

        public string Hash
        {
            get { return CreateMD5(string.Format("{0}{1}{2}", FirstName, LastName, Id)); }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        [ElasticProperty(Type = FieldType.Nested)]
        public List<Skill> Skills { get; set; }
        
        [ElasticProperty(Type = FieldType.Nested)]
        public List<Certification> Certifications { get; set; }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLowerInvariant();    // hash needs to be lowercase for www.gravatar.com to return unique images
            }
        }
    }
}