using ETMProfileEditor.ViewModel;
using LiteDB;
using Optional;
using System.Collections.Generic;

namespace ETMProfileEditor.DAL
{
    using Contract;

    public class ProfileRepository:IRepository<Profile,string>
    {
        //public void sfd()
        //{
        //    // Open database (or create if doesn't exist)
        //    using (var db = new LiteDatabase(@"C:\Temp\MyData.db"))
        //    {
        //        // Get a collection (or create, if doesn't exist)
        //        var col = db.GetCollection<Profile>();

        //        // Insert new customer document (Id will be auto-incremented)
        //        col.Insert(customer);

        //        // Update a document inside a collection
        //        customer.Name = "Joana Doe";

        //        col.Update(customer);

        //        // Index document using document Name property
        //        col.EnsureIndex(x => x.Name);

        //        // Use LINQ to query documents
        //        var results = col.Find(x => x.Name.StartsWith("Jo"));

        //        // Let's create an index in phone numbers (using expression). It's a multikey index
        //        col.EnsureIndex(x => x.Phones, "$.Phones[*]");

        //        // and now we can query phones
        //        var r = col.FindOne(x => x.Phones.Contains("8888-5555"));
        //    }
        //}

        public IEnumerable<Profile> Select()
        {
            var dir = System.IO.Directory.CreateDirectory("../../../Data");
            using (var db = new LiteDatabase(System.IO.Path.Combine(dir.FullName, "Main.litedb")))
            {
                return db.GetCollection<Profile>().FindAll();
            }
        }

        public Profile Find(string name)
        {
            var dir = System.IO.Directory.CreateDirectory("../../../Data");
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(System.IO.Path.Combine(dir.FullName, "Main.litedb")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Profile>();
                // Index document using document Name property
                col.EnsureIndex(x => x.Name);

                // Use LINQ to query documents
                return col.FindOne(x => x.Name.Equals(name));
            }
        }

        public void UpSert(Profile profile)
        {
            var dir = System.IO.Directory.CreateDirectory("../../../Data");
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(System.IO.Path.Combine(dir.FullName, "Main.litedb")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Profile>();
                // Index document using document Name property
                col.EnsureIndex(x => x.Name);

                // Use LINQ to query documents
                var result = col.FindOne(x => x.Name.Equals(profile.Name));

                if (result == null)
                {
                    col.Insert(profile);
                }
                else
                {
                    col.Update(profile);
                }
            }
        }

        public void Delete(Profile profile)
        {
            var dir = System.IO.Directory.CreateDirectory("../../../Data");
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(System.IO.Path.Combine(dir.FullName, "Main.litedb")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Profile>();
                // Index document using document Name property
                col.EnsureIndex(x => x.Name);

                // Use LINQ to query documents
                var result = col.FindOne(x => x.Name.Equals(profile.Name));

                if (result != null)
                {
                    col.Delete(result.Id);
                }
            }
        }
    }
}