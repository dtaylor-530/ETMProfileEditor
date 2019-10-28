using LiteDB;
using System.Collections.Generic;

namespace ETMProfileEditor.DAL
{
    using Contract;
    using ViewModel;

    public class ProfileRepository : IRepository<Profile, string>
    {
        private const string Directory = "Data";

        public IEnumerable<Profile> Select()
        {
            var dir = System.IO.Directory.CreateDirectory(Directory);
            using (var db = new LiteDatabase(System.IO.Path.Combine(dir.FullName, "Main.litedb")))
            {
                return db.GetCollection<Profile>().FindAll();
            }
        }

        public Profile Find(string name)
        {
            var dir = System.IO.Directory.CreateDirectory(Directory);
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
            var dir = System.IO.Directory.CreateDirectory(Directory);
            // Open database (or create if doesn't exist)
            using (var db = new LiteDatabase(System.IO.Path.Combine(dir.FullName, "Main.litedb")))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<Profile>();
                // Index document using document Name property
                col.EnsureIndex(x => x.Name);

                var result = col.FindOne(x => x.Name.Equals(profile.Name));

                if (result != null)
                {
                    //col.Update(profile);
                    col.Delete(profile.Name);
                }

                col.Insert(profile);

                //if (result == null)
                //{
                //    col.Insert(profile);
                //}
                //else
                //{
                //    col.Update(profile);
                //}
            }
        }


        public void Delete(Profile profile)
        {
            var dir = System.IO.Directory.CreateDirectory(Directory);
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