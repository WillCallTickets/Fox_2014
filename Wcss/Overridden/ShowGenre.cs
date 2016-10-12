using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class Genre
    {
    }

    public partial class GenreCollection
    {
        public bool DeleteGenreFromCollection(int idx)
        {
            Genre entity = (Genre)this.Find(idx);

            if (entity != null)
            {
                try
                {
                    Genre.Delete(idx);

                    this.Remove(entity);
                    return true;
                }
                catch (Exception e)
                {
                    _Error.LogException(e);
                    throw e;
                }
            }

            return false;
        }

        public Genre AddItemToCollection(string name, string description)
        {
            Genre newItem = new Genre();
            newItem.ApplicationId = _Config.APPLICATION_ID;
            newItem.DtStamp = DateTime.Now;
            newItem.Name = name.Trim();
            newItem.Description = description;
         
            try
            {
                newItem.Save();
                this.Add(newItem);
                return newItem;
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw e;
            }
        }
    }

    public partial class ShowGenre
    {
    }

    public partial class ShowGenreCollection
    {
        public bool DeleteShowGenreFromCollection(int idx)
        {
            ShowGenre entity = (ShowGenre)this.Find(idx);

            if (entity != null)
            {
                if (this.Count > 1)
                    this.Sort("IDisplayOrder", true);

                try
                {
                    ShowGenre.Delete(idx);

                    this.Remove(entity);
                    return true;
                }
                catch (Exception e)
                {
                    _Error.LogException(e);
                    throw e;
                }
            }

            return false;
        }
        
        public ShowGenre AddItemToCollection(int showId, int genreId)
        {
            ShowGenre newItem = new ShowGenre();
            newItem.DtStamp = DateTime.Now;
            newItem.TShowId = showId;
            newItem.TGenreId = genreId;

            try
            {
                newItem.Save();
                this.Add(newItem);

                return newItem;
            }
            catch (Exception e)
            {
                _Error.LogException(e);
                throw e;
            }
        }
    }
}
