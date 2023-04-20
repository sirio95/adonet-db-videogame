using adonet_db_videogame;

namespace adonet_db_videogame
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benvenuto nel programma di gestione dei videogiochi. Seleziona una delle seguenti opzioni per procedere");
            Console.WriteLine("1- inserisci nuovo videogioco");
            Console.WriteLine("2- cerca videogioco per ID");
            Console.WriteLine("3- cerca videogioco per nome");

            string continua = "si";
            while (continua == "si")
            {
                int scelta;
                while (!int.TryParse(Console.ReadLine(), out scelta) && (scelta < 0 || scelta > 3))
                    Console.WriteLine("Devi selezionare un numero valido");
                switch (scelta)
                {
                    case 1:
                        {
                            VideogameManager a = new VideogameManager();
                            a.InsertNewGame();
                            break;
                        }

                    case 2:
                        {
                            VideogameManager b = new VideogameManager();
                            b.SearchGameID();
                            break;
                        }
                    case 3:
                        {
                            VideogameManager c = new VideogameManager();
                            c.SearchGameName();
                            break;
                        }

                }
                Console.WriteLine("desideri svolgere altre operazioni? (si/no)");
                while(continua != "si" && continua != "no")
                    Console.WriteLine("Risposta non valida");

                Console.WriteLine("Benvenuto nel programma di gestione dei videogiochi. Seleziona una delle seguenti opzioni per procedere");
                Console.WriteLine("1- inserisci nuovo videogioco");
                Console.WriteLine("2- cerca videogioco per ID");
                Console.WriteLine("3- cerca videogioco per nome");
            }
            
        }
    }
}