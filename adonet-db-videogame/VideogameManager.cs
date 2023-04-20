using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace adonet_db_videogame
{
    
    internal class VideogameManager
    {
        
        List<VideoGame> videoGames = new List<VideoGame>();
        List<SoftwareHouse> caseprod = GetSoftwareHouses();


        public void InsertNewGame()
        {

            // raccolta dati per creare videogioco

            Console.WriteLine("Primo passo per creare un videogioco: partiamo dalle caratteristiche");
            Console.WriteLine("Indica il nome del videogioco");
            string name = Console.ReadLine();
            while (name == null || name == "")
            {
                Console.WriteLine("Scegli un nome valido");
                name = Console.ReadLine();
            }
            Console.WriteLine("Descrivi brevemente il videogioco");
            string overview = Console.ReadLine();
            while (overview == null || overview == "")
            {
                Console.WriteLine("Scegli una descrizione valida valido");
                overview = Console.ReadLine();
            }
            Console.WriteLine("Indica la data di rilascio del videogioco (formato dd/mm/yyyy)");
            DateTime release;
            while(!DateTime.TryParse(Console.ReadLine(), out release))
                Console.WriteLine("Indica una data valida");
            DateTime created = DateTime.Now;
            DateTime updated = created;

            SoftwareHouse casaProd = SelectSoftwareHouse(caseprod);
            long software_house_id = casaProd.Id;

            //inserire videogioco su DB
            string connString = "Data Source=localhost;Initial Catalog=db-videogames-query;Integrated Security=True;Pooling=False";

            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    using (SqlTransaction transazione = con.BeginTransaction("NewVideogame"))
                    {
                        cmd.Connection = con;
                        cmd.Transaction = transazione;

                        try
                        {
                            cmd.CommandText = "INSERT INTO videogames (name, overview, release_date, created_at, updated_at, software_house_id)" +
                                "VALUES (@Name, @Overview, @Release, @Created, @Updated, @SoftwareHouseId)";

                            cmd.Parameters.Add(new SqlParameter("@Name", name));
                            cmd.Parameters.Add(new SqlParameter("@Overview", overview));
                            cmd.Parameters.Add(new SqlParameter("@Release", release));
                            cmd.Parameters.Add(new SqlParameter("@Created", created));
                            cmd.Parameters.Add(new SqlParameter("@Updated", updated));
                            cmd.Parameters.Add(new SqlParameter("@SoftwareHouseId", software_house_id));

                            int numAggiunte = cmd.ExecuteNonQuery();

                            transazione.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            transazione.Rollback();
                        }
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void SearchGameID()
        {
            Console.WriteLine("Digita l'ID del gioco che vuoi cercare");
            int id;
            while(!int.TryParse(Console.ReadLine(), out id))
                Console.WriteLine("Digita un numero valido.");

            for(int i=0; i<this.videoGames.Count(); i++)
            {
                List<VideoGame> giochiId = new List<VideoGame>();
                if (videoGames[i].Id == Math.Abs(id))
                {
                    Console.WriteLine($"Il gioco cercato è: {videoGames[i].Name} del {videoGames[i].ReleaseDate}");
                    giochiId.Add(videoGames[i]);
                }
                if(giochiId == null)
                    Console.WriteLine($"Nessun gioco trovato con id pari a {id}");

            }
            

        }
        public void SearchGameName()
        {
            Console.WriteLine("Digita il nome del gioco che vuoi cercare (o parte di esso)");
            string name = Console.ReadLine();
            while (name == null || name == "")
            {
                Console.WriteLine("Assicurati di digitare almeno una lettera");
                name= Console.ReadLine();
            }
            for (int i = 0; i < this.videoGames.Count(); i++)
            {
                List<VideoGame> giochiId = new List<VideoGame>();
                if (videoGames[i].Name.Contains(name))
                {
                    Console.WriteLine($"Hai cercato {name}, contenuta in gioco n.{videoGames[i].Id} - {videoGames[i].Name}");
                    giochiId.Add(videoGames[i]);
                }
                if (giochiId == null)
                    Console.WriteLine($"Nessun gioco trovato contenente {name} nel nome");
            }
                
        }

        public List<VideoGame> GetVideoGame()
        {
            string connString = "Data Source=localhost;Initial Catalog=db-videogames-query;Integrated Security=True;Pooling=False";
            List<VideoGame> listaDB = new List<VideoGame>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string getAllGame = "SELECT * FROM videgames";
                    using (SqlCommand cmd = new SqlCommand(getAllGame, con))
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        while (res.Read())
                        {
                            long id = res.GetInt64(0);
                            string name = res.GetString(1);
                            string overview = res.GetString(2);
                            DateTime release = res.GetDateTime(3);
                            DateTime created = res.GetDateTime(4);
                            DateTime updated = res.GetDateTime(5);
                            int softHID = res.GetInt32(6);

                            VideoGame giocoN = new VideoGame(id, name, overview, release, created, updated, softHID);
                            listaDB.Add(giocoN);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return listaDB;
        }
        
        public static List<SoftwareHouse> GetSoftwareHouses()
        {
            string connString = "Data Source=localhost;Initial Catalog=db-videogames-query;Integrated Security=True;Pooling=False";
            List<SoftwareHouse> listaDB = new List<SoftwareHouse>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    string getAllGame = "SELECT * FROM software_houses";
                    using (SqlCommand cmd = new SqlCommand(getAllGame, con))
                    using (SqlDataReader res = cmd.ExecuteReader())
                    {
                        while (res.Read())
                        {
                            long id = res.GetInt64(0);
                            string name = res.GetString(1);
                            string taxId = res.GetString(2);
                            string city = res.GetString(3);
                            string country = res.GetString(4);
                            DateTime created = res.GetDateTime(5);
                            DateTime updated = res.GetDateTime(6);
                            

                            SoftwareHouse houseN = new SoftwareHouse(id, name, taxId, city, country, created, updated);
                            listaDB.Add(houseN);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return listaDB;
        }
        public SoftwareHouse SelectSoftwareHouse(List<SoftwareHouse> lista)
        {
            SoftwareHouse houseScelta = null;

            Console.WriteLine("Seleziona la casa di produzione del videogioco");
            foreach (SoftwareHouse a in lista)
                Console.WriteLine($"Numero: {a.Id} - {a.Name} - {a.Country}");
            int scelta;
            while(!int.TryParse(Console.ReadLine(), out scelta) && (scelta < 0 || scelta >6 ))
                Console.WriteLine("Scegli un numero valido");
            switch (scelta)
            {
                case 1:
                    houseScelta= lista[0];
                    break;
                case 2:
                    houseScelta = lista[1];
                    break;
                case 3:
                    houseScelta = lista[2];
                    break;
                case 4:
                    houseScelta = lista[3];
                    break;
                case 5:
                    houseScelta = lista[4];
                    break;
                case 6:
                    houseScelta = lista[5];
                    break;
            }
            return houseScelta;
        }
    }

    public class VideoGame
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Overview { get; set; }
        public DateTime ReleaseDate{ get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get;set; }
        public int SoftwareHouseID { get; set; }

        public VideoGame(long id, string name, string overview, DateTime releaseDate, DateTime createdAt, DateTime updatedAt, int softwareHouseID)
        {
            Id = id;
            Name = name;
            Overview = overview;
            ReleaseDate = releaseDate;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            SoftwareHouseID = softwareHouseID;
        }
    }
    public class SoftwareHouse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TaxID { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public SoftwareHouse(long id, string name, string taxid, string city, string country, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Name = name;
            TaxID = taxid;
            City = city;
            Country = country;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
