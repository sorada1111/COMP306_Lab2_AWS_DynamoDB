using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301270677_prathan__Lab02
{
    [DynamoDBTable("Bookshelf")]
    public class Book
    {
        [DynamoDBHashKey] // Partition key
        public string? ISBN { get; set; }

        [DynamoDBRangeKey] // Sort key
        public string? UserName { get; set; }

        [DynamoDBProperty]
        public string? BookTitle { get; set; }

        [DynamoDBProperty]
        public List<string>? Author { get; set; }

        [DynamoDBProperty]
        public string? BookmarkTime { get; set; }

        [DynamoDBProperty]
        public int BookmarkPage { get; set; }

        [DynamoDBProperty]
        public string? PdfTitle { get; set; }
    }
}
