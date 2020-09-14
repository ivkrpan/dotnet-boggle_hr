using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;

namespace BoggleService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class BoggleService : IBoggleService
    {
        private static System.Timers.Timer aTimer;

        private Dictionary<string, int> nadjeno_rijeci = new Dictionary<string, int>();
        private Dictionary<string, string> ploca =   new Dictionary<string, string>();
        private Dictionary<string, string> tempPloca = new Dictionary<string, string>();
        private List<KeyValuePair<string, string>> koristene_rijeci = new List<KeyValuePair<string, string>>();

        private string[] bazenSlova;
        private int vrijeme = 0;


        public bool napraviIgru()
        {
            //Generira novu igru (ploću), resetira podatke
            string[] suglasnici = { "b", "c", "č", "ć", "d", "đ", "dž", "f", "g", "h", "j", "k", "l", "lj", "m", "n", "nj", "p", "r", "š", "t", "v", "z", "ž" };
            string[] samoglasnici = { "a", "e", "i", "o", "u" };

            string[] tempSlova = { };

            for (int n = 0; n < 4; n++)
                for (int i = 0; i < samoglasnici.Length; i++)
                {
                    Array.Resize(ref tempSlova, tempSlova.Length + 1);
                    tempSlova[tempSlova.Length - 1] = samoglasnici[i];
                }

            for (int n = 0; n < 2; n++)
                for (int i = 0; i < suglasnici.Length; i++)
                {
                    Array.Resize(ref tempSlova, tempSlova.Length + 1);
                    tempSlova[tempSlova.Length - 1] = suglasnici[i];
                }

            tempSlova = tempSlova.Take(tempSlova.Count() - 1).ToArray();

            Random rnd = new Random();
            bazenSlova = tempSlova.OrderBy(x => rnd.Next()).ToArray();

            //kreiraj novu plocu
            ploca.Clear();
            int m = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    string id = i + "-" + j;
                    ploca.Add(id, bazenSlova[m]);
                    m++;
                }
            }

            try
            {
                int b = nadjeno_rijeci[dohvatiSesiju()];
            }
            catch
            {
                nadjeno_rijeci.Add(dohvatiSesiju(), 0);
            }

            if (aTimer == null)
            {
                aTimer = new System.Timers.Timer(1000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = false;
            }

            koristene_rijeci.Clear();
            nadjeno_rijeci.Clear();
            vrijeme = 180;

            return true;
        }

        public IEnumerable<String> dohvatiSlova()
        {
            //Dovaća ploću slova koja se trenutno igra
            IEnumerable<String> aktivnaSlova;

            aktivnaSlova = bazenSlova.Take(25);
            aTimer.Enabled = true;

            return aktivnaSlova;
        }

        public string provjeriRijec(string rijec)
        {
            //Pronalazi mogući početak riječi na ploči, te poziva funkciju za provjeru i validiranje riječi, na temelju kojih boduje riječ
            int nadjenoRijeci;
            try
            {
                nadjenoRijeci = nadjeno_rijeci[dohvatiSesiju()];
            }
            catch { nadjenoRijeci = 0; }

            if (rijec.Length < 3)
                return "Minimalno 3 slova! Nađeno riječi: " + nadjenoRijeci;

            if (vrijeme == 0)
                return "Igra nije aktivna!";

            int nadjenoSlova = 0;
            int bodovi = 0;

            tempPloca.Clear();

            foreach (KeyValuePair<string, string> kvp in ploca)
                tempPloca.Add(kvp.Key, kvp.Value);

            string prvoSlovo = rijec[0].ToString();

            if (rijec.Length > 1)
                if (prvoSlovo == "n" || prvoSlovo == "l")
                {
                    if (rijec[1].ToString() == "j")
                        prvoSlovo = prvoSlovo + rijec[1];
                }
                else if (prvoSlovo == "d" && rijec[1].ToString() == "ž")
                    prvoSlovo = prvoSlovo + "ž";

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (prvoSlovo == ploca[i + "-" + j])
                    {
                        if (pronadjiRijec(rijec, i, j, ref nadjenoSlova))
                        {
                            KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(dohvatiSesiju(), rijec);

                            if (koristene_rijeci.Contains(kvp))
                            {
                                return "Ponovljena riječ! Nađeno riječi: " + nadjeno_rijeci[dohvatiSesiju()];
                            }

                            if (validirajRijec(rijec))
                            {
                                bodovi = rijecBodova(rijec);
                            }
                            else
                            {
                                return "Riječ nije valjana! Nađeno riječi: " + nadjeno_rijeci[dohvatiSesiju()];
                            }

                            try
                            {
                                nadjeno_rijeci[dohvatiSesiju()] += 1;
                            }
                            catch
                            {
                                nadjeno_rijeci.Add(dohvatiSesiju(), 1);
                            }

                            koristene_rijeci.Add(kvp);

                            return "Bodova: " + bodovi + " Nađeno riječi: " + nadjeno_rijeci[dohvatiSesiju()];
                        }
                    }
                }
            }

            return "Ne postoji riječ! Nađeno riječi: " + nadjenoRijeci; ;
        }


        public int preostaloVremena()
        {
            //Vraća info. koliko je sekundi ostalo do kraja igre
            return vrijeme;
        }


        public List<KeyValuePair<string, string>> popisRezultata()
        {
            //Vraća popis rezultata na kraju igre
            List<KeyValuePair<string, string>> bodovi = new List<KeyValuePair<string, string>>();

            int n = 1;
            foreach (KeyValuePair<string, int> kvp in nadjeno_rijeci)
            {

                if (kvp.Key == dohvatiSesiju())
                    bodovi.Add(new KeyValuePair<string, string>("IGRAČ " + n + " (JA)", "Br. riječi: " + kvp.Value));
                else
                    bodovi.Add(new KeyValuePair<string, string>("IGRAČ " + n, "Br. riječi: " + kvp.Value));

                int ukb = 0;

                foreach (KeyValuePair<string, string> kr in koristene_rijeci)
                {
                    var brr = from cust in koristene_rijeci where cust.Value == kr.Value select cust.Key;

                    if (kvp.Key == kr.Key)
                        if (brr.Count() == 1)
                        {
                            ukb += rijecBodova(kr.Value);
                            bodovi.Add(new KeyValuePair<string, string>("  " + kr.Value, "Bodova: " + rijecBodova(kr.Value)));
                        }
                        else
                            bodovi.Add(new KeyValuePair<string, string>(" *" + kr.Value, "Bodova: 0"));
                }

                bodovi.Add(new KeyValuePair<string, string>("", "UKUPNO: " + ukb));
                n++;
            }
            return bodovi;
        }

        private string dohvatiSesiju()
        {
            //Dohvaća oznaku sesije od aktivnog korisnika
            var sessionId = OperationContext.Current.SessionId;
            return sessionId.Substring(9);
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Funkcija koja se okida na timmer
            if (vrijeme > 0)
                vrijeme--;
            else
                aTimer.Enabled = false;
        }

        private string[] ucitajDatoteku(string name)
        {
            //Dohvaća sadržaj datoteke
            string[] fileContents = { };

            if (System.IO.File.Exists(name))
            {
                fileContents = System.IO.File.ReadAllLines(name);
            }
            return fileContents;
        }

        private bool pronadjiRijec(string rijec, int x, int y, ref int bodovi)
        {
            //Rekurzivna funkcija traži postoji li riječ (kombinacija slova) na zadanoj ploči
            if (rijec.Length == 0)
                return true;
            else
            {
                string prvoSlovo = rijec[0].ToString();

                if (rijec.Length > 1)

                    if (prvoSlovo == "n" || prvoSlovo == "l")
                    {
                        if (rijec[1].ToString() == "j")
                            prvoSlovo = prvoSlovo + rijec[1];
                    }
                    else if (prvoSlovo == "d" && rijec[1].ToString() == "ž")
                        prvoSlovo = prvoSlovo + "ž";


                if (prvoSlovo == tempPloca[x + "-" + y])
                {
                    tempPloca[x + "-" + y] = "-";
                    bodovi = bodovi + 1;

                    try
                    {
                        if (x > 0 && y > 0)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x - 1, y - 1, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    try
                    {
                        if (x > 0)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x - 1, y, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    try
                    {
                        if (x > 0 && y < 4)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x - 1, y + 1, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    try
                    {
                        if (x < 4 && y > 0)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x + 1, y - 1, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    try
                    {
                        if (x < 4)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x + 1, y, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    try
                    {
                        if (x < 4 && y < 4)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x + 1, y + 1, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    try
                    {
                        if (y > 0)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x, y - 1, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    try
                    {
                        if (y < 4)
                            if (pronadjiRijec(rijec.Substring(prvoSlovo.Length), x, y + 1, ref bodovi) == true)
                                return true;
                    }
                    catch { }

                    return false;
                }
            }
            return false;
        }

        private bool validirajRijec(string rijec)
        {
            //Provjerava riječi sa popisom dozvoljenih riječi u .wdb datotekama u folderu rijeci
            string startFolder = @AppDomain.CurrentDomain.BaseDirectory + "\\rijeci";
 
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);

            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.wdb", System.IO.SearchOption.AllDirectories);

            string searchTerm = @rijec;

            var queryMatchingFiles = from file in fileList
                where file.Extension == ".wdb"
                let fileText = ucitajDatoteku(file.FullName)
                where fileText.Contains(searchTerm)
                select file.FullName;

            if (queryMatchingFiles.Count() > 0)
                return true;

            return false;
        }


        private int rijecBodova(string rijec)
        {
            //Bodovanje rijeci
            int bodovi = 0;
            rijec.Replace("nj", "-");
            rijec.Replace("dž", "-");
            rijec.Replace("lj", "-");

            if (rijec.Length == 3 || rijec.Length == 4)
                bodovi = 1;
            else if (rijec.Length == 5)
                bodovi = 2;
            else if (rijec.Length == 6)
                bodovi = 3;
            else if (rijec.Length == 7)
                bodovi = 5;
            else if (rijec.Length >= 8)
                bodovi = 11;

            return bodovi;
        }
    }
}
