using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static void Main()
    {
        string adminpass = "admin123";
        //budowanie połączenia z bazą przez connection string
        string connectionString = "Host=horton.db.elephantsql.com;Username=horahqkv;Password=ViMyNKtDNz4F959L64dTUMo2gPQ_i14z;Database=horahqkv";

        using (var connection = new NpgsqlConnection(connectionString))
        {

            try
            {
                connection.Open(); //Otwieranie połączenia
                Console.WriteLine("Połączono z bazą danych.");


                while (true)
                {
                    MenuUżytkownik(connection, adminpass);
                }

            }
            catch (Exception ex) //powiadomienie w wypadku problemu z połączeniem
            {
                Console.WriteLine($"Błąd połączenia z bazą danych: {ex.Message}");
            }
        }
    }

    #region Admin

    #region Menus

    public static void MenuTablic(NpgsqlConnection connection)
    {
        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            WyswietlWszystkieTablice(connection);
            Console.ForegroundColor = ConsoleColor.Magenta;
            
            Console.WriteLine("Menu Tabel:");
            Console.WriteLine("1. Wyświetl wszystkie tablice");
            Console.WriteLine("2. Zmien nazwe kolumny");
            Console.WriteLine("3. Utwórz nową kolumnę");
            Console.WriteLine("4. Usuń kolumnę");
            Console.WriteLine("0. Powrót do głównego menu");

            Console.Write("Wybierz opcję: ");
            if (int.TryParse(Console.ReadLine(), out int opcja))
            {
                switch (opcja)
                {
                    case 1:
                        WyswietlWszystkieTablice(connection);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Wciśnij cokolwiek by kontynuować");
                        Console.ResetColor();
                        break;

                    case 2:
                        ZmienNazweKolumny(connection);
                        break;

                    case 3:
                        UtworzNowaKolumne(connection);
                        break;

                    case 4:
                        UsuńKolumnę(connection);
                        break;

                    case 0:
                        return;

                    default:
                        Console.WriteLine("Niepoprawna opcja. Spróbuj ponownie.");
                        Thread.Sleep(1000);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
            }
            if (opcja == 0) { break; }
        }
    }

    public static void MenuAdmina(NpgsqlConnection connection, string adminpass)
    {
        while (true)
        {
            Console.Clear();
            WyświetlNagłówekAdmin();

            Console.ForegroundColor = ConsoleColor.Cyan;
            WyswietlWszystkieTablice(connection);
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine("Menu Administratora:");
            Console.WriteLine("1. Zarządzaj pojazdami");
            Console.WriteLine("2. Zarządzaj klientami");
            Console.WriteLine("3. Zarządzaj wypożyczeniami");
            Console.WriteLine("4. Zarządzaj tablicami");
            Console.WriteLine("0. Wyloguj się z konsoli admina");
            Console.ResetColor();

            Console.Write("Wybierz opcję: ");
            if (int.TryParse(Console.ReadLine(), out int opcja))
            {

                switch (opcja)
                {
                    case 1:
                        Console.Clear();
                        MenuPojazdy(connection, adminpass);
                        break;

                    case 2:
                        Console.Clear();
                        MenuKlienci(connection);
                        break;

                    case 3:
                        Console.Clear();
                        MenuWypozyczenia(connection);
                        break;
                    case 4:
                        Console.Clear();
                        MenuTablic(connection);
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine("Niepoprawna opcja. Spróbuj ponownie.");
                        Thread.Sleep(1000);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
            }
            if(opcja == 0) { break; }
        }
    }
    
    public static void MenuPojazdy(NpgsqlConnection connection, string adminpass)
    {
        Console.Clear();
        while (true)
        {
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            WyświetlWszystkiePojazdyAdmin(connection);
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine("Menu Pojazdy:");
            Console.WriteLine("1. Dodaj nowy pojazd");
            Console.WriteLine("2. Usuń pojazd");
            Console.WriteLine("3. Edytuj dane pojazdu");
            Console.WriteLine("4. Wyświetl pojazd po ID");
            Console.WriteLine("0. Powrót do głównego menu");

            Console.Write("Wybierz opcję: ");
            if (int.TryParse(Console.ReadLine(), out int opcja))
            {
                switch (opcja)
                {
                    case 1:
                        Console.Clear();
                        UtwórzNowyPojazd(connection);
                        Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                        Console.ReadLine();
                        break;

                    case 2:
                        while (true)
                        {
                            Console.Clear();
                            WyświetlWszystkiePojazdyAdmin(connection);
                            Console.Write("Podaj ID pojazdu do usunięcia: ");

                            if (int.TryParse(Console.ReadLine(), out int idPojazduUsun))
                            {
                                try
                                {
                                    UsuńPojazd(connection, idPojazduUsun, adminpass);

                                    break;
                                }
                                catch
                                {
                                    Console.WriteLine("Nie można usunąć pojazdu który ma rekordy w tablicy wypozyczenia");
                                    Thread.Sleep(1000);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowe ID. Wprowadź poprawną liczbę całkowitą.");
                            }
                        }

                        Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                        Console.ReadLine();
                        break;

                    case 3:
                        Console.Clear();
                        WyświetlWszystkiePojazdyAdmin(connection);
                        EdytujPojazd(connection);
                        Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                        Console.ReadLine();
                        break;

                    case 4:
                        while (true)
                        {
                            Console.Clear();
                            WyświetlWszystkiePojazdyAdmin(connection);
                            Console.Write("Podaj ID pojazdu do wyświetlenia: ");

                            if (int.TryParse(Console.ReadLine(), out int idPojazduWyświetl))
                            {
                                WyświetlPojazdPoID(connection, idPojazduWyświetl);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowe ID. Wprowadź poprawną liczbę całkowitą.");
                            }
                        }

                        Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                        Console.ReadLine();
                        break;

                    case 0:
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("Niepoprawna opcja. Spróbuj ponownie.");
                        Thread.Sleep(1000);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
            }

            if (opcja == 0) { break; }
        }
    }

    public static void MenuKlienci(NpgsqlConnection connection)
    {
        while (true)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            WyswietlWszystkichKlientowAdmin(connection);
            Console.ForegroundColor = ConsoleColor.Magenta;

;
            Console.WriteLine("Menu Klienci:");
            Console.WriteLine("1. Dodaj nowego klienta");
            Console.WriteLine("2. Usuń klienta");
            Console.WriteLine("3. Edytuj dane klienta");
            Console.WriteLine("0. Powrót do poprzedniego menu");

            Console.Write("Wybierz opcję: ");
            if (int.TryParse(Console.ReadLine(), out int opcja))
            {
                switch (opcja)
                {
                    case 1:
                        Console.Clear();
                        UtwórzNowegoKlienta(connection);
                        break;

                    case 2:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        WyswietlWszystkichKlientowAdmin(connection);
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        
                        Console.Write("Podaj ID klienta do usunięcia: ");

                        if (int.TryParse(Console.ReadLine(), out int idKlientaUsun))
                        {
                            try
                            {
                                UsuńKlienta(connection, idKlientaUsun);
                            }
                            catch
                            {
                                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Nie można usunąć klienta który ma rekordy na tablicy wypożyczenia");
                                Thread.Sleep (1000);
                                Console.ResetColor();
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Nieprawidłowe ID. Wprowadź poprawną liczbę całkowitą.");
                            Console.ResetColor();
                        }
                        break;

                    case 3:
                        Console.Clear();
                        EdytujKlienta(connection);
                        break;

                    case 0:
                        break;

                    default:
                        Console.WriteLine("Niepoprawna opcja. Spróbuj ponownie.");
                        Thread.Sleep(1000);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
            }
            if (opcja == 0) { break; }
        }
    }

    public static void MenuWypozyczenia(NpgsqlConnection connection)
    {
        Console.Clear();
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            WyświetlWszystkieWypożyczenia(connection);
            Console.ForegroundColor = ConsoleColor.Magenta;
          
            Console.WriteLine("Menu Wypożyczeń:");
            Console.WriteLine("1. Utwórz nowe wypożyczenie");
            Console.WriteLine("2. Edytuj istniejące wypożyczenie");
            Console.WriteLine("3. Usuń istniejące wypożyczenie");
            Console.WriteLine("0. Powrót do poprzedniego menu");

            Console.Write("Wybierz opcję: ");
            if (int.TryParse(Console.ReadLine(), out int opcja))
            {
                switch (opcja)
                {
                    case 1:
                        Console.Clear();
                        UtwórzNoweWypozyczenie(connection);
                        break;
                    case 2:
                        Console.Clear();
                        WyświetlWszystkieWypożyczenia(connection);
                        EdytujWypozyczenie(connection);
                        Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                        Console.ReadLine();
                        break;
                    case 3:
                        Console.Clear();
                        WyświetlWszystkieWypożyczenia(connection);
                        Console.WriteLine("Proszę podać ID wypożyczenia do usunięcia:");
                        if (int.TryParse(Console.ReadLine(), out int idToDelete))
                        {
                            UsuńWypożyczenie(connection, idToDelete);
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowe ID. Wprowadź poprawną liczbę całkowitą.");
                        }
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                        break;

                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
            }
            if (opcja == 0) { break; }

        }
    }

    #endregion

    #region Metody pojazdy
    //tworzenie nowego pojazdu 
    public static void UtwórzNowyPojazd(NpgsqlConnection connection)
    {
        //Deklaracja zmiennych
        string nr_rejestracji;
        string model;
        string cena;

        //pozyskanie nazwy pojazdu
        model = ZwróćModelPojazdu();

        //pozyskanie numeru rejestracji
        nr_rejestracji = ZwróćNumerRejestracyjny();

        //pozyskanie ceny wynajmu pojazdu
        cena = ZwróćCenęWynajmu(); 

        //tworzenie komendy SQL przy użyciu podanych zmiennych
        string query = $"INSERT INTO pojazdy (nr_rejestracji, model, cena, czy_wypozyczone, czy_uszkodzone) VALUES('{nr_rejestracji}', '{model}', '{cena}', false, false)";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        {
            try
            {
                command.ExecuteNonQuery(); //wywołanie funkcji które wykonuje utowrzoną komendę
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("Dodano nowy pojazd.");
            }
            catch (Exception ex) //komunikat w razie błędu
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"Błąd podczas dodawania pojazdu: {ex.Message}"); 
            }
        }
    }

    //Edycja informacji o wybranym pojeździe
    public static void EdytujPojazd(NpgsqlConnection connection)
    {
        while (true)
        {
            Console.WriteLine("Proszę podać id pojazdu do edycji: "); //prośba o podanie id pojazdu do edycji

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                do
                {
                    // Menu wyboru akcji
                    Console.WriteLine("Wybierz akcję:");
                    Console.WriteLine("1. Aktualizuj rejestrację pojazdu");
                    Console.WriteLine("2. Aktualizuj model pojazdu");
                    Console.WriteLine("3. Aktualizuj cenę pojazdu");
                    Console.WriteLine("4. Aktualizuj stan wynajęcia pojazdu");
                    Console.WriteLine("5. Aktualizuj stan uszkodzenia pojazdu");
                    Console.WriteLine("0. Zakończ edycję");


                    if (int.TryParse(Console.ReadLine(), out int opcja))
                    {
                        switch (opcja)
                        {
                            case 1:
                                AktualizujRejestracjęPojazdu(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 2:
                                AktualizujModelPojazdu(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 3:
                                AktualizujCenęWynajmuPojazdu(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 4:
                                AktualizujWynajęciePojazdu(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 5:
                                AktualizujUszkodzeniePojazdu(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 0:
                                Console.WriteLine("Opuszczanie menu edycji");
                                // żeby ta opcja wychodziła do menu "głównego" musimy je zrobić w pętli tak jak przy projekcie z programowania
                                Thread.Sleep(1000);
                                break;
                            default:
                                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    }

                    if (opcja == 0) { break; }
                } while (!SprawdzCzyIdPojazduIstnieje(connection, id));

                break;
            }
            else
            {
                Console.WriteLine("Proszę podać liczbę całkowitą.");
            }
        }
    }

    //usuwanie pojazdu
    public static void UsuńPojazd(NpgsqlConnection connection, int id, string adminpass)
    {

        do
        {  
            Console.WriteLine($"Wpisz hasło by potwierdzić usunięcie pojazdu o id {id}"); //prośba o potwierdzenie usunięcia przez podanie hasła
            if (Console.ReadLine() == adminpass)
            {
                //tworzenie komendy SQL która usunie pojazd o podanym id
                string query = $"DELETE FROM pojazdy WHERE id_pojazdu = {id}";

                using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
                using (var reader = command.ExecuteReader()) { } //wywołanie funkcji które wykonuje utowrzoną komendę
                Console.WriteLine($"\n\nUsunięto pojazd o id {id}");
            }
            else
            {
                Console.WriteLine("Niepoprawne hasło");
            }

            break;
            
        }while (true);

    }

    //Wyświetlanie pojazdów
    public static void WyświetlWszystkiePojazdyAdmin(NpgsqlConnection connection)
    {
        //tworzenie komendy SQl która wszystkie rekordy z tablicy pojazdy
        string query = "SELECT * FROM pojazdy";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        using (var reader = command.ExecuteReader()) //wywołanie funkcji które wykonuje utowrzoną komendę
        {
            Console.WriteLine("Dane pojazdów:");

            while (reader.Read()) //Wyświetlanie pojazdów w pętli
            {
                //pozyskanie zmiennych przez odwołanie się do typu zmiennych i numeru kolumny w ktorej się znajdują
                int id = reader.GetInt32(0);    //pozyskanie zmiennej typu int z "zerowej" kolumny
                string nr_rejestracji = reader.GetString(1);    //pozyskanie zmiennej typu string z pierwszej kolumny
                string pojazd = reader.GetString(2);    //pozyskanie zmiennej typu string z drugiej kolumny
                double cena = reader.GetDouble(3);  //pozyskanie zmiennej typu double z trzeciej kolumny
                bool czy_wypozyczone = reader.GetBoolean(4);    //pozyskanie zmiennej typu bool z czwartej kolumny
                bool czy_uszkodzone = reader.GetBoolean(5);    //pozyskanie zmiennej typu bool z piątej kolumny

                //Administrator widzi pełne informacje o pojazdach
                Console.WriteLine("========================================================");
                Console.WriteLine($"Pojazd: {pojazd}   /|\\ ID Pojazdu: {id}" +
                    $"\n Nr. Rejestracji: {nr_rejestracji} \n Cena wypozyczenia: {cena}" +
                    $"\n  Wypożyczone? => {czy_wypozyczone} \n  Uszkodzone? => {czy_uszkodzone}");
                Console.WriteLine("========================================================");
            }
        }
    }

    //wyświetlanie pojedynczego pojazdu
    public static void WyświetlPojazdPoID(NpgsqlConnection connection, int id)
    {
        string query = $"SELECT * FROM pojazdy WHERE id_pojazdu = '{id}'";
        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        using (var reader = command.ExecuteReader()) //wywołanie funkcji które wykonuje utowrzoną komendę
        {
            Console.WriteLine("Dane pojazdu:");
            while (reader.Read()) //Wyświetlanie pojazdów w pętli
            {
                //pozyskanie zmiennych przez odwołanie się do typu zmiennych i numeru kolumny w ktorej się znajdują
                string nr_rejestracji = reader.GetString(1);    //pozyskanie zmiennej typu string z pierwszej kolumny
                string pojazd = reader.GetString(2);    //pozyskanie zmiennej typu string z drugiej kolumny
                double cena = reader.GetDouble(3);  //pozyskanie zmiennej typu double z trzeciej kolumny
                bool czy_wypozyczone = reader.GetBoolean(4);    //pozyskanie zmiennej typu bool z czwartej kolumny
                bool czy_uszkodzone = reader.GetBoolean(5);    //pozyskanie zmiennej typu bool z piątej kolumny

                //Administrator widzi pełne informacje o pojazdach
                Console.WriteLine("========================================================");
                Console.WriteLine($"Pojazd: {pojazd}   /|\\ ID Pojazdu: {id}" +
                    $"\n Nr. Rejestracji: {nr_rejestracji} \n Cena wypozyczenia: {cena}" +
                    $"\n  Wypożyczone? => {czy_wypozyczone} \n  Uszkodzone? => {czy_uszkodzone}");
                Console.WriteLine("========================================================");
            }
        }
    }

    #region Aktualizacja danych

    //aktualizajca rejestracji pojazdu
    private static void AktualizujRejestracjęPojazdu(NpgsqlConnection connection, int id)
    {
        //pozyskanie nowej zmiennej
        string nowaRejestracja = ZwróćNumerRejestracyjny();

        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE pojazdy SET nr_rejestracji = '{nowaRejestracja}' WHERE id_pojazdu = '{id}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Numer rejestracji pojazdu o ID {id} został zaktualizowany.");
        }
    }

    //aktualizacja modelu
    private static void AktualizujModelPojazdu(NpgsqlConnection connection, int id)
    {
        //pozyskanie nowej zmiennej
        string nowyModel = ZwróćModelPojazdu();
        
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonych zmiennych
        string query = $"UPDATE pojazdy SET model = '{nowyModel}' WHERE id_pojazdu = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine($"Model pojazdu o ID {id} został zaktualizowane.");
        }
    }

    //aktualizacja ceny wynajmu
    private static void AktualizujCenęWynajmuPojazdu(NpgsqlConnection connection, int id)
    {
        //pozyskanie nowej zmiennej
        string nowaCena = ZwróćCenęWynajmu();

        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE pojazdy SET cena = '{nowaCena}' WHERE id_pojazdu = '{id}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Cena wynajmu pojazdu o ID {id} została zaktualizowane.");
        }
    }

    //aktualizacja stanu wynajęcia
    private static void AktualizujWynajęciePojazdu(NpgsqlConnection connection, int id)
    {
        bool noweCzy_wynajęty = ZwróćStanWynajmu();
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE pojazdy SET czy_wypozyczone = '{noweCzy_wynajęty}' WHERE id_pojazdu = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Stan wynajęcia pojazdu o ID {id} został zaktualizowany.");
        }
    }

    //aktualizacja stanu uszkdozenia
    private static void AktualizujUszkodzeniePojazdu(NpgsqlConnection connection, int id)
    {
        bool noweCzy_uszkodzony = ZwróćStanUszkodzenia();
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE pojazdy SET czy_uszkodzone = '{noweCzy_uszkodzony}' WHERE id_pojazdu = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Stan uszkodzenia pojazdu o ID {id} został zaktualizowany.");
        }
    }
    #endregion

    #endregion

        #region Metody klienci

    //utwóz nowego klienta (patrz utwórznowypojazd)
    public static void UtwórzNowegoKlienta(NpgsqlConnection connection)
    {
        //Deklaracja zmiennych
        string imie;
        string nazwisko;
        string numer_telefonu;
        string pesel;

        //pozyskanie imienia
        imie = ZwróćImie();

        //pozyskanie nazwiska
        nazwisko = ZwróćNazwisko();

        //pozyskanie numeru telefonu
        numer_telefonu = ZwróćNumerTelefonu();

        //pozyskanie peselu
        pesel = ZwróćPesel();

        //tworzenie komendy SQL przy użyciu podanych zmiennych
        string query = $"INSERT INTO klienci (imie, nazwisko, numer_telefonu, pesel) VALUES('{imie}', '{nazwisko}', '{numer_telefonu}', {pesel})";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        {
            try
            {
                command.ExecuteNonQuery(); //wywołanie funkcji które wykonuje utowrzoną komendę
                Console.WriteLine("Dodano nowego klienta.");
            }
            catch (Exception ex) //komunikat w razie błędu
            {
                Console.WriteLine($"Błąd podczas dodawania klienta: {ex.Message}");
            }
        }
    }
    //edytuj informacje klienta o danym id (patrz edytujpojazd)
    public static void EdytujKlienta(NpgsqlConnection connection)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        WyswietlWszystkichKlientowAdmin(connection);
        Console.ForegroundColor = ConsoleColor.Magenta;

        Console.WriteLine("Proszę podać id klienta do edycji: ");
              
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            do
            {
                // Menu wyboru akcji
                Console.WriteLine("Wybierz akcję:");
                Console.WriteLine("1. Aktualizuj imie klienta");
                Console.WriteLine("2. Aktualizuj nazwisko klienta");
                Console.WriteLine("3. Aktualizuj numer telefonu klienta");
                Console.WriteLine("4. Aktualizuj pesel klienta");
                Console.WriteLine("5. Zakończ edycję");


                if (int.TryParse(Console.ReadLine(), out int choice)) //wybór funckji przez podanie liczby
                {
                    switch (choice)
                    {
                        case 1:
                            AktualizujImieKlienta(connection, id);                  
                            break;
                        case 2:
                            AktualizujNazwiskoKlienta(connection, id);
                            break;
                        case 3:
                            AktualizujNumerKlienta(connection, id);
                            break;
                        case 4:
                            AktualizujPeselKlienta(connection, id);
                            break;
                        case 5:
                            Console.WriteLine("Opuszczanie menu edycji");
                            // żeby ta opcja wychodziła do menu "głównego" musimy je zrobić w pętli tak jak przy projekcie z programowania
                            break;
                        default:
                            Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                }


                } while (!SprawdzCzyIdKlientaIstnieje(connection, id));
            }
            else
            {
                Console.WriteLine("Proszę podać liczbę całkowitą.");
            }
        
    }
    //usun informacje o kliencie (patrz usunpojazd0
    public static void UsuńKlienta(NpgsqlConnection connection, int id)
    {
        string adminpass = "admin123";
        do
        {
            Console.WriteLine($"Wpisz hasło by potwierdzić usunięcie klienta o id {id}"); //prośba o potwierdzenie usunięcia przez podanie hasła
            if (Console.ReadLine() == adminpass)
            {
                //tworzenie komendy SQL która usunie pojazd o podanym id
                string query = $"DELETE FROM klienci WHERE id_klienta = {id}";

                using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
                using (var reader = command.ExecuteReader()) { } //wywołanie funkcji które wykonuje utowrzoną komendę
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"\n\nUsunięto klienta o id {id}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("Niepoprawne hasło");
                Console.ResetColor();
            }

            break;

        } while (true);

    }
    //wyswietl wszystkich klientow (patrz wyswietlwsyztskiepojazdyadmin)
    public static void WyswietlWszystkichKlientowAdmin(NpgsqlConnection connection)
    {
        
        //tworzenie komendy SQl która wszystkie rekordy z tablicy pojazdy
        string query = "SELECT * FROM klienci";
        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        using (var reader = command.ExecuteReader()) //wywołanie funkcji które wykonuje utowrzoną komendę
        {
            Console.WriteLine("Dane klientów:");

            while (reader.Read()) //Wyświetlanie klientów w pętli
            {
                //pozyskanie zmiennych przez odwołanie się do typu zmiennych i numeru kolumny w ktorej się znajdują
                int id = reader.GetInt32(0);    //pozyskanie zmiennej typu int z "zerowej" kolumny
                string imie = reader.GetString(1);    //pozyskanie zmiennej typu string z pierwszej kolumny
                string nazwisko = reader.GetString(2);    //pozyskanie zmiennej typu string z drugiej kolumny
                string numer_telefonu = reader.GetString(3);  //pozyskanie zmiennej typu double z trzeciej kolumny
                string pesel = reader.GetString(4);    //pozyskanie zmiennej typu bool z czwartej kolumny


                //Administrator widzi pełne informacje o kliencie
                Console.WriteLine("========================================================");
                Console.WriteLine($"Imię: {imie}  Nazwisko: {nazwisko} /|\\ ID Klienta: {id}" +
                    $"\n Nr. Telefonu: {numer_telefonu} \n Pesel: {pesel}");
                Console.WriteLine("========================================================");
            }
        }
        
    }

    //wystwietl pojedynczego klienta po id (patrz wyswietlpojazdpoid)
    public static void WyświetlKlientaPoID(NpgsqlConnection connection, int id)
    {
        //utworzenie komendy SQL która pozyska dane klienta o podanym id
        string query = $"SELECT * FROM klienci WHERE id_klienta = {id}";
        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        using (var reader = command.ExecuteReader()) //wywołanie funkcji które wykonuje utowrzoną komendę
        {
            Console.WriteLine("Dane klienta:");
            while (reader.Read()) //Wyświetlanie klientów w pętli
            {
                //pozyskanie zmiennych przez odwołanie się do typu zmiennych i numeru kolumny w ktorej się znajdują
                string imie = reader.GetString(1);    //pozyskanie zmiennej typu string z pierwszej kolumny
                string nazwisko = reader.GetString(2);    //pozyskanie zmiennej typu string z drugiej kolumny
                string numer_telefonu = reader.GetString(3);  //pozyskanie zmiennej typu double z trzeciej kolumny
                string pesel = reader.GetString(4);    //pozyskanie zmiennej typu bool z czwartej kolumny

                //Administrator widzi pełne informacje o pojazdach
                Console.WriteLine("========================================================");
                Console.WriteLine($"Imię: {imie}  Nazwisko: {nazwisko} /|\\ ID Klienta: {id}" +
                    $"\n Nr. Telefonu: {numer_telefonu} \n Pesel: {pesel}");
                Console.WriteLine("========================================================");
            }
        }
    }

    #region Aktualizacja danych 
    private static void AktualizujImieKlienta(NpgsqlConnection connection, int id)
    {
        string noweImie = ZwróćImie();
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonych zmiennych
        string query = $"UPDATE klienci SET imie = '{noweImie}' WHERE id_klienta = '{id}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine($"Klient o ID {id} został zaktualizowany.");
            WyświetlKlientaPoID(connection, id);
        }
    }

    private static void AktualizujNazwiskoKlienta(NpgsqlConnection connection, int id)
    {
        string noweNazwisko = ZwróćNazwisko();
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonych zmiennych
        string query = $"UPDATE klienci SET nazwisko = '{noweNazwisko}' WHERE id_klienta = '{id}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine($"Klient o ID {id} został zaktualizowany.");
            WyświetlKlientaPoID(connection, id);
        }
    }

    private static void AktualizujNumerKlienta(NpgsqlConnection connection, int id)
    {
        string nowyNumer = ZwróćNumerTelefonu();
        
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonych zmiennych
        string query = $"UPDATE klienci SET numer_telefonu = '{nowyNumer}' WHERE id_klienta = '{id}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine($"Klient o ID {id} został zaktualizowany.");
            WyświetlKlientaPoID(connection, id);
        }
    }

    private static void AktualizujPeselKlienta(NpgsqlConnection connection, int id)
    {
        string nowyPesel = ZwróćPesel();
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonych zmiennych
        string query = $"UPDATE klienci SET pesel = '{nowyPesel}' WHERE id_klienta = '{id}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery();
            Console.WriteLine($"Klient o ID {id} został zaktualizowany.");
            WyświetlKlientaPoID(connection, id);
        }
    }
    #endregion

    #endregion

        #region Metody wypozyczenia
    public static void UtwórzNoweWypozyczenie(NpgsqlConnection connection)
    {
        //Deklaracja zmiennych
        int id_pojazdu;
        int id_klienta;
        string data_wypozyczenia;

        //pozyskanie id pojazdu
        id_pojazdu = ZwróćIDPojazdu(connection);

        //pozyskanie id klienta
        id_klienta = ZwróćIDKlienta(connection);

        //pozyskanie daty wynajmu pojazdu
        data_wypozyczenia = ZwróćDzisiejsząDatę();

     
        //tworzenie komendy SQL przy użyciu podanych zmiennych
        string query = $"INSERT INTO wypozyczenia (id_pojazdu, id_klienta, data_wypozyczenia, czy_oplacone) VALUES('{id_pojazdu}', '{id_klienta}', '{data_wypozyczenia}', false)";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        {
            try
            {
                command.ExecuteNonQuery(); //wywołanie funkcji które wykonuje utowrzoną komendę
                Console.WriteLine("Dodano nowe wypozyczenie.");
            }
            catch (Exception ex) //komunikat w razie błędu
            {
                Console.WriteLine($"Błąd podczas dodawania wypozyczenia: {ex.Message}");
            }
        }
    }

    public static void EdytujWypozyczenie(NpgsqlConnection connection)
    {
        while (true)
        {
            Console.WriteLine("Proszę podać id wypożyczenia do edycji: "); //prośba o podanie id wypozyczenia do edycji

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                do
                {
                    // Menu wyboru akcji
                    Console.WriteLine("Wybierz akcję:");
                    Console.WriteLine("1. Aktualizuj id pojazdu wynajmu pojazdu");
                    Console.WriteLine("2. Aktualizuj id klienta wynajmu pojazdu");
                    Console.WriteLine("3. Aktualizuj datę wypożyczenia wynajmu pojazdu");
                    Console.WriteLine("4. Aktualizuj datę zwrotu wynajmu pojazdu");
                    Console.WriteLine("5. Aktualizuj stan opłacenia wynajmu pojazdu");
                    Console.WriteLine("6. Zakończ edycję");


                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                AktualizujIDPojazduWypozyczenia(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 2:
                                AktualizujIDKlientaWypozyczenia(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 3:
                                AktualizujDatęWypozyczenia(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 4:
                                AktualizujDatęZwrotuWypozyczenia(connection, id);
                                Console.Clear();
                                break;
                            case 5:
                                AktualizujOpłaceniePojazdu(connection, id);
                                Console.Clear();
                                WyświetlPojazdPoID(connection, id);
                                break;
                            case 6:
                                Console.WriteLine("Opuszczanie menu edycji");
                                // żeby ta opcja wychodziła do menu "głównego" musimy je zrobić w pętli tak jak przy projekcie z programowania
                                break;
                            default:
                                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    }

                    if (choice == 6) { break; }
                } while (!SprawdzCzyIdWypozyczeniaIstnieje(connection, id));
                break;
            }
            else
            {
                Console.WriteLine("Proszę podać liczbę całkowitą.");
            }
        }
    }

    //Wyświetlanie wypozyczeń
    public static void WyświetlWszystkieWypożyczenia(NpgsqlConnection connection)
    {
        //tworzenie komendy SQl która wszystkie rekordy z tablicy pojazdy
        string query = "SELECT * FROM wypozyczenia ";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        using (var reader = command.ExecuteReader()) //wywołanie funkcji które wykonuje utowrzoną komendę
        {
            Console.WriteLine("Dane wypozczyczenia:");
            while (reader.Read()) //Wyświetlanie pojazdów w pętli
            {
                //pozyskanie zmiennych przez odwołanie się do typu zmiennych i numeru kolumny w ktorej się znajdują
                int id_wypozyczenia = reader.GetInt32(0);
                int id_pojazdu = reader.GetInt32(1);
                int id_klienta = reader.GetInt32(2); ;
                string data_wypozyczenia = reader.GetDateTime(3).ToShortDateString();
                DateTime? data_zwrotuNullable = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);
                string data_zwrotu = data_zwrotuNullable.HasValue ? data_zwrotuNullable.Value.ToShortDateString() : "NULL";
                bool czy_opłacone = reader.GetBoolean(5);

                //Administrator widzi pełne informacje o pojazdach
                Console.WriteLine("========================================================");
                Console.WriteLine($"ID wynajmu: {id_wypozyczenia}" +
                    $"\nId pojazdu: {id_pojazdu} || Id klienta: {id_klienta}" +
                    $"\nData wypozyczenia: {data_wypozyczenia}    || Data zwrotu: {data_zwrotu}" +
                    $"\n Opłacone? => {czy_opłacone}");
                Console.WriteLine("========================================================");
            }
        }
    }

    //wyswietl wypozyczenie po id
    public static void WyświetlWypozyczeniePoID(NpgsqlConnection connection, int id)
    {
        string query = $"SELECT * FROM wypozyczenia WHERE id_wypozyczenia = {id}";
        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        using (var reader = command.ExecuteReader()) //wywołanie funkcji które wykonuje utowrzoną komendę
        {
            Console.WriteLine("Dane wypozczyczenia:");
            while (reader.Read()) //Wyświetlanie pojazdów w pętli
            {
                //pozyskanie zmiennych przez odwołanie się do typu zmiennych i numeru kolumny w ktorej się znajdują
                int id_pojazdu = reader.GetInt32(1);    //pozyskanie zmiennej typu int z pierwszej kolumny
                int id_klienta = reader.GetInt32(2); ;    //pozyskanie zmiennej typu int z drugiej kolumny
                string data_wypozyczenia = reader.GetDateTime(3).ToShortDateString(); //pozyskanie zmiennej typu string z trzeciej kolumny
                DateTime? data_zwrotuNullable = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4);
                string data_zwrotu = data_zwrotuNullable.HasValue ? data_zwrotuNullable.Value.ToShortDateString() : "NULL";
                bool czy_opłacone = reader.GetBoolean(5);    //pozyskanie zmiennej typu bool z piątej kolumny

                //Administrator widzi pełne informacje o pojazdach
                Console.WriteLine("========================================================");
                Console.WriteLine($"ID wynajmu: {id} " +
                    $"\nId pojazdu: {id_pojazdu} || Id klienta: {id_klienta}" +
                    $"\nData wypozyczenia: {data_wypozyczenia}    || Data zwrotu: {data_zwrotu}" +
                    $"\nOpłacone? => {czy_opłacone}");
                Console.WriteLine("========================================================");
            }
        }
    }

    //usun wypozyczenie
    public static void UsuńWypożyczenie(NpgsqlConnection connection, int id)
    {
        string adminpass = "admin123";
        do
        {
            Console.WriteLine($"Wpisz hasło by potwierdzić usunięcie dane wypozyczeniea o id {id}"); //prośba o potwierdzenie usunięcia przez podanie hasła
            if (Console.ReadLine() == adminpass)
            {
                //tworzenie komendy SQL która usunie pojazd o podanym id
                string query = $"DELETE FROM wypozyczenia WHERE id_wypozyczenia = {id}";

                using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
                using (var reader = command.ExecuteReader()) { } //wywołanie funkcji które wykonuje utowrzoną komendę
                Console.WriteLine($"\n\nUsunięto dane wypozyczenia o id {id}");
            }
            else
            {
                Console.WriteLine("Niepoprawne hasło");
            }

            break;

        } while (true);

    }

    #region Aktualizacja danych

    //aktualizajca Id pojazdu wynajmu
    private static void AktualizujIDPojazduWypozyczenia(NpgsqlConnection connection, int id)
    {
        //pozyskanie nowej zmiennej
        int noweid = ZwróćIDPojazdu(connection);

        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE wypozyczenia SET id_pojazdu = '{noweid}' WHERE id_wypozyczenia = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Id pojazdu dla wypozyczenia o ID {id} zostało zaktualizowane.");
        }
    }

    //aktualizajca id klienta wynajmu
    private static void AktualizujIDKlientaWypozyczenia(NpgsqlConnection connection, int id)
    {
        //pozyskanie nowej zmiennej
        int noweid = ZwróćIDKlienta(connection);

        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE wypozyczenia SET id_klienta = '{noweid}' WHERE id_wypozyczenia = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Id klienta dla wypozyczenia o ID {id} zostało zaktualizowane.");
        }
    }

    //aktualizajca daty wypozyczenia wynajmu
    private static void AktualizujDatęWypozyczenia(NpgsqlConnection connection, int id)
    {
        //pozyskanie nowej zmiennej
        string nowadata = ZwróćDatęWypożyczenia();

        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE wypozyczenia SET data_wypozyczenia = '{nowadata}' WHERE id_wypozyczenia = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Data wynajmu dla wypozyczenia o ID {id} została zaktualizowana.");
        }
    }

    //aktualizajca daty zwrotu wynajmu
    private static void AktualizujDatęZwrotuWypozyczenia(NpgsqlConnection connection, int id)
    {
        //pozyskanie nowej zmiennej
        string nowadata = ZwróćDatęZwrotu(connection, id);

        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE wypozyczenia SET data_zwrotu = '{nowadata}' WHERE id_wypozyczenia = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Data zwrotu dla wypozyczenia o ID {id} została zaktualizowana.");
            WyświetlPojazdPoID(connection, id);
        }
    }

    //aktualizacja stanu uszkdozenia
    private static void AktualizujOpłaceniePojazdu(NpgsqlConnection connection, int id)
    {
        bool noweCzy_Opłacone = ZwróćStanOpłacenia();
        //tworzenie poleceina SQL które zaktualizuje pojazd przy uzyciu wprowadzonej zmiennej
        string query = $"UPDATE wypozyczenia SET czy_oplacone= '{noweCzy_Opłacone}' WHERE id_wypozyczenia = {id}";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.ExecuteNonQuery(); //wykonanie polecenia
            Console.WriteLine($"Stan opłacenia pojazdu o ID {id} został zaktualizowany.");
        }
    }
    #endregion

    #endregion

        #region metody tablic
    static void WyswietlWszystkieTablice(NpgsqlConnection connection)
    {
        WyswietlTabele(connection, "wypozyczenia");
        WyswietlTabele(connection, "pojazdy");
        WyswietlTabele(connection, "klienci");
    }

    static void WyswietlTabele(NpgsqlConnection connection, string tableName)
    {
        try
        {
            // Sprawdzenie czy tabela istnieje
            string checkTableQuery = $"SELECT table_name FROM information_schema.tables WHERE table_name = '{tableName}'";
            using (var checkTableCommand = new NpgsqlCommand(checkTableQuery, connection))
            {
                using (var tableReader = checkTableCommand.ExecuteReader())
                {
                    if (!tableReader.HasRows)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"Tabela '{tableName}' nie istnieje.");
                        Console.ResetColor();
                        return;
                    }
                }
            }

            // Pobranie struktury tabeli
            string describeTableQuery = $"SELECT column_name, data_type FROM information_schema.columns WHERE table_name = '{tableName}'";
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"Struktura tabeli '{tableName}':");
            using (var describeTableCommand = new NpgsqlCommand(describeTableQuery, connection))
            {
                using (var describeTableReader = describeTableCommand.ExecuteReader())
                {
                    while (describeTableReader.Read())
                    {
                        string columnName = describeTableReader.GetString(0);
                        string dataType = describeTableReader.GetString(1);
                        Console.ForegroundColor = ConsoleColor.Cyan;  Console.WriteLine($"Kolumna: {columnName}, Typ danych: {dataType}");
                        Console.ResetColor();
                    }
                }
            }

            // Pobranie zawartości tabeli
            string selectTableQuery = $"SELECT * FROM {tableName}";
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine($"\nZawartość tabeli '{tableName}':");
            using (var selectTableCommand = new NpgsqlCommand(selectTableQuery, connection))
            {
                using (var selectTableReader = selectTableCommand.ExecuteReader())
                {
                    while (selectTableReader.Read())
                    {
                        for (int i = 0; i < selectTableReader.FieldCount; i++)
                        {
                            string columnName = selectTableReader.GetName(i);
                            string columnValue = selectTableReader.IsDBNull(i) ? "NULL" : selectTableReader.GetValue(i).ToString();
                            Console.ForegroundColor = ConsoleColor.Cyan; Console.Write($"{columnName}: {columnValue}\t");
                        }
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }
            }

            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas wyświetlania tabeli '{tableName}': {ex.Message}");
        }
    }

    private static void ZmienNazweKolumny(NpgsqlConnection connection)
    {
        Console.WriteLine("podaj nazwę tabeli którą chcesz edytować: ");
        string tableName = Console.ReadLine();
        
        WyswietlTabele(connection, tableName);
        
        Console.WriteLine("podaj nazwę kolumny którą chcesz edytować: ");
        string oldColumnName = Console.ReadLine();

        Console.WriteLine("podaj nową nazwę kolumny: ");
        string newColumnName = Console.ReadLine();

        try
        {
            // Sprawdzenie czy tabela i kolumna istnieją
            string checkQuery = $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tableName}' AND column_name = '{oldColumnName}'";
            using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
            {
                using (var reader = checkCommand.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"Tabela '{tableName}' lub kolumna '{oldColumnName}' nie istnieje.");
                        return;
                    }
                }
            }

            // Zmiana nazwy kolumny w tabeli
            string renameQuery = $"ALTER TABLE {tableName} RENAME COLUMN {oldColumnName} TO {newColumnName}";
            using (var renameCommand = new NpgsqlCommand(renameQuery, connection))
            {
                Console.Clear();
                renameCommand.ExecuteNonQuery();
                Console.WriteLine($"Zmieniono nazwę kolumny z '{oldColumnName}' na '{newColumnName}' w tabeli '{tableName}'.");
                WyswietlTabele(connection, tableName);
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas zmiany nazwy kolumny: {ex.Message}");
        }
    }

    static void UtworzNowaKolumne(NpgsqlConnection connection)
    {
        Console.WriteLine("podaj nazwę tabeli którą chcesz edytować: ");
        string tableName = Console.ReadLine();

        WyswietlTabele(connection, tableName);

        Console.WriteLine("podaj nazwę kolumny którą chcesz utworzyć: ");
        string newColumnName = Console.ReadLine();
       
        Console.WriteLine("podaj typ danych którym ma być kolumna: ");
        string columnType = Console.ReadLine();

        try
        {
            // Sprawdzenie czy tabela istnieje
            string checkTableQuery = $"SELECT table_name FROM information_schema.tables WHERE table_name = '{tableName}'";
            using (var checkTableCommand = new NpgsqlCommand(checkTableQuery, connection))
            {
                using (var tableReader = checkTableCommand.ExecuteReader())
                {
                    if (!tableReader.HasRows)
                    {
                        Console.WriteLine($"Tabela '{tableName}' nie istnieje.");
                        return;
                    }
                }
            }

            // Sprawdzenie czy kolumna już istnieje
            string checkColumnQuery = $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tableName}' AND column_name = '{newColumnName}'";
            using (var checkColumnCommand = new NpgsqlCommand(checkColumnQuery, connection))
            {
                using (var columnReader = checkColumnCommand.ExecuteReader())
                {
                    if (columnReader.HasRows)
                    {
                        Console.WriteLine($"Kolumna '{newColumnName}' już istnieje w tabeli '{tableName}'.");
                        return;
                    }
                }
            }

            // Dodanie nowej kolumny do tabeli
            string addColumnQuery = $"ALTER TABLE {tableName} ADD COLUMN {newColumnName} {columnType}";
            using (var addColumnCommand = new NpgsqlCommand(addColumnQuery, connection))
            {
                Console.Clear();
                addColumnCommand.ExecuteNonQuery();
                Console.WriteLine($"Dodano nową kolumnę '{newColumnName}' do tabeli '{tableName}' o typie danych '{columnType}'.");
                WyswietlTabele(connection, tableName);
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas tworzenia nowej kolumny: {ex.Message}");
        }
    }

    static void UsuńKolumnę(NpgsqlConnection connection)
    {
        Console.WriteLine("podaj nazwę tabeli którą chcesz edytować: ");
        string tableName = Console.ReadLine();

        WyswietlTabele(connection, tableName);

        Console.WriteLine("podaj nazwę kolumny którą chcesz usunąć: ");
        string columnName = Console.ReadLine();

        try
        {
            // Sprawdzenie czy tabela istnieje
            string checkTableQuery = $"SELECT table_name FROM information_schema.tables WHERE table_name = '{tableName}'";
            using (var checkTableCommand = new NpgsqlCommand(checkTableQuery, connection))
            {
                using (var tableReader = checkTableCommand.ExecuteReader())
                {
                    if (!tableReader.HasRows)
                    {
                        Console.WriteLine($"Tabela '{tableName}' nie istnieje.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            // Sprawdzenie czy kolumna istnieje
            string checkColumnQuery = $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tableName}' AND column_name = '{columnName}'";
            using (var checkColumnCommand = new NpgsqlCommand(checkColumnQuery, connection))
            {
                using (var columnReader = checkColumnCommand.ExecuteReader())
                {
                    if (!columnReader.HasRows)
                    {
                        Console.WriteLine($"Kolumna '{columnName}' nie istnieje w tabeli '{tableName}'.");
                        Console.ReadKey();
                        return;
                    }
                }
            }

            // Usunięcie kolumny z tabeli
            string dropColumnQuery = $"ALTER TABLE {tableName} DROP COLUMN {columnName}";
            using (var dropColumnCommand = new NpgsqlCommand(dropColumnQuery, connection))
            {
                Console.Clear();
                dropColumnCommand.ExecuteNonQuery();
                Console.WriteLine($"Usunięto kolumnę '{columnName}' z tabeli '{tableName}'.");
                WyswietlTabele(connection, tableName);
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas usuwania kolumny: {ex.Message}");
        }
    }

    #endregion

    #region inne    
    private static void WyświetlNagłówekAdmin()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("============================================================================================================================================");
        Console.WriteLine("==========//       \\===========||               \\=======||          \\=======//          |=====||      |======||         \\=======||      |===");
        Console.WriteLine("=========//    __   \\==========||     _____      \\======||           \\=====//           |=====||      |======||          \\======||      |===");
        Console.WriteLine("========//    /=\\\\   \\=========||     |====\\\\     \\=====||            \\===//            |=====||      |======||           \\=====||      |===");
        Console.WriteLine("=======//    /===\\\\   \\========||     |=====\\\\     \\====||             \\=//             |=====||      |======||            \\====||      |===");
        Console.WriteLine("======//    /=====\\\\   \\=======||     |======||     |===||      |\\             /|       |=====||      |======||     |\\      \\===||      |===");
        Console.WriteLine("=====//     _________   \\======||     |======||     |===||      |\\\\           /||       |=====||      |======||     |\\\\      \\==||      |===");
        Console.WriteLine("====//     /========\\\\   \\=====||     |======||     /===||      |=\\\\         /=||       |=====||      |======||     |=\\\\      \\=||      |===");
        Console.WriteLine("===//     /==========\\\\   \\====||     |=====//     /====||      |==\\\\       /==||       |=====||      |======||     |==\\\\      \\||      |===");
        Console.WriteLine("==//     /============\\\\   \\===||     |====//     /=====||      |===\\\\     /===||       |=====||      |======||     |===\\\\              |===");
        Console.WriteLine("=//     /==============\\\\   \\==||                /======||      |====\\\\   /====||       |=====||      |======||     |====\\\\             |===");
        Console.WriteLine("============================================================================================================================================");

        Console.ResetColor();
    }
    private static bool SprawdzCzyIdPojazduIstnieje(NpgsqlConnection connection, int id)
    {
        // budowanie polecenia SQL które zliczy ilość pojazdów o danym id
        string query = $"SELECT COUNT(*) FROM pojazdy WHERE id_pojazdu = {id}";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        {
            int iloscRekordow = Convert.ToInt32(command.ExecuteScalar()); //wykonanie komendy
            return iloscRekordow > 0; //zwrócenie true, jeśli istnieje rekord o takim id
        }
    }
    private static bool SprawdzCzyIdKlientaIstnieje(NpgsqlConnection connection, int id)
    {
        // budowanie polecenia SQL które zliczy ilość klientów o danym id
        string query = $"SELECT COUNT(*) FROM klienci WHERE id_klienta = {id}";

        using (var command = new NpgsqlCommand(query, connection))  //zdefiniowanie komendy w połączeniu
        {
            int iloscRekordow = Convert.ToInt32(command.ExecuteScalar());   //wykonanie komendy
            return iloscRekordow > 0;   //zwrócenie true, jeśli istnieje rekord o takim id
        }
    }

    private static bool SprawdzCzyIdWypozyczeniaIstnieje(NpgsqlConnection connection, int id)
    {
        // budowanie polecenia SQL które zliczy ilość klientów o danym id
        string query = $"SELECT COUNT(*) FROM wypozyczenia WHERE id_wypozyczenia = '{id}'";

        using (var command = new NpgsqlCommand(query, connection))  //zdefiniowanie komendy w połączeniu
        {
            int iloscRekordow = Convert.ToInt32(command.ExecuteScalar());   //wykonanie komendy
            return iloscRekordow > 0;   //zwrócenie true, jeśli istnieje rekord o takim id
        }
    }

    #endregion

    #endregion

    #region Użytkownik

    #region Menu

    public static void MenuUżytkownik(NpgsqlConnection connection, string adminpass)
    {
        //menu dla metod użytkownika + tajne logowanie dla admina
        while (true)
        {
            Console.Clear();

            WyświetlNagłówekUser();

            WyświetlPojazdyUżytkownik(connection);

            Console.WriteLine("Menu główne:");
            Console.WriteLine("1. Wypożycz pojazd");
            Console.WriteLine("2. Zwróć pojazd");

            Console.Write("Wybierz opcję: ");
            string opcja = Console.ReadLine();

            switch (opcja)
            {
                case "1":
                    Console.Clear();
                    WyświetlPojazdyUżytkownik(connection);
                    WypożyczPojazd(connection);
                    Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                    Console.ReadLine();
                    break;

                case "2":
                    Console.Clear();
                    OddajPojazd(connection);
                    Console.WriteLine("Naciśnij Enter, aby kontynuować...");
                    Console.ReadLine();
                    break;

                case "admin":
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Uruchomiono konsolę admina.\nWpisz hasło by kontynuować: ");
                    Console.BackgroundColor = ConsoleColor.Red;
                    string haslo = Console.ReadLine();
                    if (haslo == adminpass)
                    {
                        Console.ResetColor();
                        MenuAdmina(connection, adminpass);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Niepoprawne hasło, logowanie nieudane");
                        Thread.Sleep(1000);
                        Console.ResetColor();
                    }
                    return;

                default:
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Niepoprawna opcja. Spróbuj ponownie.");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    break;


            }
        }


    }

    #endregion
   
    #region Metody

    //Wyświetlanie Headera
    public static void  WyświetlNagłówekUser()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;

        Console.WriteLine("====================================================================================================");
        Console.WriteLine("=======|     ||======|     ||=======/     ____    \\\\====|          ||=====|           \\\\============");
        Console.WriteLine("=======|     ||======|     ||======|     ||==|     ||===|     _____||=====|   ______   \\\\===========");
        Console.WriteLine("=======|     ||======|     ||======|     ||==|_____||===|    ||===========|   ||====\\   \\\\==========");
        Console.WriteLine("=======|     ||======|     ||======|      \\\\============|    ||_____======|   ||====|    ||=========");
        Console.WriteLine("=======|     ||======|     ||=======\\            \\\\=====|          ||=====|   ||====|    ||=========");
        Console.WriteLine("=======|     ||======|     ||========\\_____       \\\\====|     _____||=====|   ||===/    //==========");
        Console.WriteLine("=======|     ||======|     ||==============\\       ||===|    ||===========|   ____     //===========");
        Console.WriteLine("=======|     ||======|     ||=====|     ||=|       ||===|    ||_____======|   ||==\\    \\\\===========");
        Console.WriteLine("========\\    \\\\=====/     //======|     ||=|       ||===|          ||=====|   ||===\\    \\\\==========");
        Console.WriteLine("=========\\               //========\\              //====|          ||=====|   ||====\\    \\\\=========");
        Console.WriteLine("====================================================================================================");

        Console.ResetColor();
    }

    //Wyświetlanie pojazdów dla użytkownika
    public static void WyświetlPojazdyUżytkownik(NpgsqlConnection connection)
    {
        //tworzenie komendy SQl która znajduje nieuszkodzone i niewypożyczone auta
        string query = "SELECT * FROM pojazdy WHERE czy_wypozyczone = false And czy_uszkodzone = false"; 

        using (var command = new NpgsqlCommand(query, connection))
        using (var reader = command.ExecuteReader()) //wywołanie funkcji które wykonuje utowrzoną komendę
        {
            Console.WriteLine("Pojazdy do wynajęcia:");

            while (reader.Read())
            {
                //pozyskanie zmiennych przez odwołanie się do typu zmiennych i numeru kolumny w ktorej się znajdują
                string pojazd = reader.GetString(2);    //pozyskanie zmiennej typu string z drugiej kolumny
                double cena = reader.GetDouble(3);  //pozyskanie zmiennej typu double z trzeciej kolumny

                //Użytkownik widzi tylko podstawowe informacje o pojeździe
                Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("========================================================");
                Console.ForegroundColor = ConsoleColor.DarkCyan; Console.WriteLine($"Pojazd: {pojazd}");
                Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"Cena wypozyczenia: {cena}");
                Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine("========================================================");
                Console.ResetColor();
            }
        }
    }

    public static void WypożyczPojazd(NpgsqlConnection connection)
    {
        // Prośba o podanie modelu pojazdu
        string pojazd = ZwróćModelPojazdu();

        // Sprawdzenie, czy pojazd o podanym modelu istnieje
        int id_pojazdu = ZwróćIDPoModelu(connection, pojazd);

        if (id_pojazdu == -1)
        {
            Console.WriteLine("Pojazd o podanym modelu nie istnieje. Spróbuj ponownie.");
            return;
        }


        // Sprawdzenie, czy pojazd jest dostępny do wypożyczenia
        if (!SprawdzDostępnośćPojazdu(connection, id_pojazdu))
        {
            Console.WriteLine("Pojazd jest niedostępny do wypożyczenia.");
            return;
        }
        Console.WriteLine("sprawdzono dostępność");

        // Pozyskanie danych od użytkownika
        string imie = ZwróćImie();
        string nazwisko = ZwróćNazwisko();
        string numer_telefonu = ZwróćNumerTelefonu();
        string pesel = ZwróćPesel();

        // Sprawdzenie, czy klient o podanych danych istnieje
        int id_klienta = ZwróćIdKlientaPoPełnychDanych(connection, imie, nazwisko, pesel, numer_telefonu);

        if (id_klienta == -1)
        {
            DodajKlienta(connection, imie, nazwisko, numer_telefonu, pesel);
        }

        // Potwierdzenie wypożyczenia
        Console.WriteLine($"Czy na pewno chcesz wypożyczyć pojazd {pojazd}? (aby potwierdzić wpisz '{pojazd}')");
        
        string potwierdzenie = Console.ReadLine();
        string frazapotwierdzenie = pojazd.ToString();

        if (potwierdzenie != $"{frazapotwierdzenie}")
        {
            Console.WriteLine("Anulowano wypożyczenie.");
            return;
        }

        // Oznaczenie pojazdu jako wypożyczonego
        OznaczPojazdJakoWypożyczony(connection, id_pojazdu);

        id_klienta = ZwróćIdKlientaPoPełnychDanych(connection, imie, nazwisko, pesel, numer_telefonu);

        // Utworzenie rekordu w tablicy wypożyczenia
        DodajWypożyczenie(connection, id_pojazdu, id_klienta);

        Console.WriteLine("Pojazd został wypożyczony.");
    }

    public static void OddajPojazd(NpgsqlConnection connection)
    {
        string numer_telefonu = ZwróćNumerTelefonu();
        string pesel = ZwróćPesel();
        int id_Klienta = ZwróćIdKlientaPoSkróconychDanych(connection, pesel, numer_telefonu);

        string numer_rejestracyjny = ZwróćNumerRejestracyjny();
        int id_pojazdu = ZwróćIdPojazduPoRejestracji(connection, numer_rejestracyjny);

        if (id_Klienta == -1 || id_pojazdu == -1)
        {
            Console.WriteLine("Nie znaleziono wynajętych pojazdów odpowiadających wprowadzonym danym");
            return;
        }

        bool czy_oplacone = Zapłać();
        if (czy_oplacone)
        {

            // Sprawdzenie czy istnieje wypożyczenie o podanych id klienta i pojazdu z datą zwrotu równą null
            string query = $"SELECT id_wypozyczenia FROM wypozyczenia WHERE id_klienta = {id_Klienta} AND id_pojazdu = {id_pojazdu} AND data_zwrotu IS NULL LIMIT 1";

            using (var command = new NpgsqlCommand(query, connection))
            {
                int idWypozyczenia = Convert.ToInt32(command.ExecuteScalar());

                if (idWypozyczenia == 0)
                {
                    Console.WriteLine("Nie znaleziono aktywnego wypożyczenia dla podanych danych.");
                    return;
                }

                string data_zwrotu = ZwróćDzisiejsząDatę();

                query = $"UPDATE wypozyczenia SET data_zwrotu = '{data_zwrotu}' WHERE id_wypozyczenia = '{idWypozyczenia}'";

                using (var updateCommand = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        updateCommand.ExecuteNonQuery();
                        Console.WriteLine("Pojazd został zwrócony.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd podczas zapisywania daty zwrotu: {ex.Message}");
                    }
                }

                query = $"UPDATE pojazdy SET czy_wypozyczone = false WHERE id_pojazdu = {id_pojazdu}";
                using (var updateCommand = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        updateCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd podczas aktualizacji flagi czy_wypozyczone: {ex.Message}");
                    }
                }

                query = $"UPDATE wypozyczenia SET czy_oplacone = true WHERE id_pojazdu = {id_pojazdu} AND id_klienta = {id_Klienta}";
                using (var updateCommand = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        updateCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd podczas aktualizacji flagi czy_opłacone: {ex.Message}");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Płatność nie przeszła; proszę spróbować ponownie");
            return;
        }

    }
    #endregion

    #region inne 
    public static bool SprawdzDostępnośćPojazdu(NpgsqlConnection connection, int idPojazdu)
    {
        string query = $"SELECT COUNT(id_pojazdu) FROM pojazdy WHERE id_pojazdu = '{idPojazdu}' AND czy_wypozyczone = false AND czy_uszkodzone = false";

        using (var command = new NpgsqlCommand(query, connection))
        {
            int ilośćRekordów = Convert.ToInt32(command.ExecuteScalar());

            return ilośćRekordów > 0;
        }
    }

    public static void DodajKlienta(NpgsqlConnection connection, string imie, string nazwisko, string numer_telefonu, string pesel)
    {
        string query = $"INSERT INTO klienci (imie, nazwisko, numer_telefonu, pesel) VALUES('{imie}', '{nazwisko}', '{numer_telefonu}', '{pesel}')";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        {
            try
            {
                command.ExecuteNonQuery(); //wywołanie funkcji które wykonuje utowrzoną komendę
            }
            catch (Exception ex) //komunikat w razie błędu
            {
                Console.WriteLine($"Błąd podczas dodawania klienta: {ex.Message}");
            }
        }
    }

    public static void OznaczPojazdJakoWypożyczony(NpgsqlConnection connection, int id_pojazdu)
    {
        string query = $"UPDATE pojazdy SET czy_wypozyczone = true WHERE id_pojazdu = '{id_pojazdu}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            try
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"Pojazd o ID {id_pojazdu} został oznaczony jako wypożyczony.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas oznaczania pojazdu jako wypożyczony: {ex.Message}");
            }
        }
    }

    public static void DodajWypożyczenie(NpgsqlConnection connection, int id_pojazdu, int id_klienta)
    {
        string data_wypozyczenia = ZwróćDzisiejsząDatę();
        string query = $"INSERT INTO wypozyczenia (id_pojazdu, id_klienta, data_wypozyczenia, czy_oplacone) VALUES('{id_pojazdu}', '{id_klienta}', '{data_wypozyczenia}', false)";

        using (var command = new NpgsqlCommand(query, connection)) //zdefiniowanie komendy w połączeniu
        {
            try
            {
                command.ExecuteNonQuery(); //wywołanie funkcji które wykonuje utowrzoną komendę
            }
            catch (Exception ex) //komunikat w razie błędu
            {
                Console.WriteLine($"Błąd podczas wypożyczania: {ex.Message}");
            }
        }
    }

    public static bool Zapłać()
    {
        bool poszło;
        int czyprzejdzie = new Random().Next(0, 2);
        if (czyprzejdzie == 1 || czyprzejdzie == 2)
        {
            poszło = true;
            return poszło;
        }
        else
        {
            poszło = false;
            return poszło;
        }
    }

    #endregion

    #endregion

    #region Zwracanie zmiennych

    #region pojazdy

        //pozyskanie Id pojazdu po modelu
        public static int ZwróćIDPoModelu(NpgsqlConnection connection, string model)
        {
            int idPojazdu = -1;

            string query = $"SELECT id_pojazdu FROM pojazdy WHERE model = '{model}' LIMIT 1";

            using (var command = new NpgsqlCommand(query, connection))
            {

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idPojazdu = reader.GetInt32(0);
                        return idPojazdu;
                    }
                }
            }
        
            return idPojazdu;
        }

        //pozyskanie id przez rejestrację
        public static int ZwróćIdPojazduPoRejestracji(NpgsqlConnection connection, string numer_rejestracji)
        {
            int idPojazdu = -1;

            string query = $"SELECT id_pojazdu FROM pojazdy WHERE nr_rejestracji = '{numer_rejestracji}' LIMIT 1";

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idPojazdu = reader.GetInt32(0);
                        return idPojazdu;
                    }
                }
            }
            return idPojazdu;
        }

        //pozyskiwanie modelu pojazdu
        public static string ZwróćModelPojazdu()
        {
            //deklaracja zmiennych
            string model;

            do
            {
                Console.WriteLine("Proszę wprowadzić model pojazdu:"); //prośba o wprowadzenie danych przez użytkownika
                model = Console.ReadLine();

            } while (string.IsNullOrEmpty(model)); //powtórzenie pętli jeśli dane będą puste

            return model; //zwrot modelu
        }

        //Pozyskiwanie numeru rejestracyjnego 
        public static string ZwróćNumerRejestracyjny()
        {
            //deklaracja zmiennych
            string nrRejestracyjny;
        
            do
            {
                Console.Write("Podaj numer rejestracyjny (dokładnie 7 znaków): "); //prośba o wprowadzenie danych przez użytkownika
                nrRejestracyjny = Console.ReadLine();

            } while (nrRejestracyjny.Length != 7); //sprawdzenie czy użytkownik podał dokładnie 7 znaków; jeśli nie, pętla sie powtózy i użytkownik ponownie zostanie poproszony o podanie danych

            return nrRejestracyjny; //zwrot numeru rejestracyjnego
        }

        //Pozyskiwanie ceny wynajmu przez metodę
        public static string ZwróćCenęWynajmu()
        {
            //deklaracja zmiennych
            double cenaDouble;
            string cenaString;

            do
            {
                Console.WriteLine("Proszę wprowadzić cenę wynajmu pojazdu (liczbę)"); //prośba o wprowadzenie danych przez użytkownika
                if (double.TryParse(Console.ReadLine(),out cenaDouble)) //sprawdzenie czy użytkownik podał poprawnie wartości liczbowe
                {
                    break; //jeśli zakończone powodzeniem wychodzi z petli
                }
                else //jeśli nie, wyświetla poniższy komunikat
                {
                    Console.WriteLine("Niepoprawny format ceny. Proszę podać liczbę w formacie dziesiętnym (1,1)."); 
                }

            } while (true);

            cenaString = cenaDouble.ToString().Replace(',', '.'); //zmiana wartości liczbowej w zmienną typu string i zmiana przecinka na kropkę, aby wartość została przyjęta jako zmienna numeric w bazie

            return cenaString; //zwrot ceny w formacie string
        }

        //pozyskiwanie stanu wynajmu pojazdu
        public static bool ZwróćStanWynajmu()
        {
            //deklaracja zmiennych
            bool stanWynajmu;
            
            do
            {
                Console.WriteLine("Proszę o wprowadzenie stanu wynajmu pojazdu (true/false)"); //prośba o wprowadzenie danych przez użytkownika
                if (bool.TryParse(Console.ReadLine(), out stanWynajmu)) //sprawdzenie czy użytkownik podał poprawnie wartości 
                {
                    break;  //jeśli zakończone powodzeniem wychodzi z petli
                }
                else  //jeśli nie, wyświetla poniższy komunikat
                {
                    Console.WriteLine("Niepoprawny format; Proszę podać wartość true lub false");
                }
            } while (true);

            return stanWynajmu; //zwrot ceny w formacie string
        }

        //pozyskiwanie stanu uszkodzenia pojazdu
        public static bool ZwróćStanUszkodzenia()
        {
            //deklaracja zmiennych
            bool stanUszkodzenia;

            do
            {
                Console.WriteLine("Proszę o wprowadzenie stanu uszkodzenia pojazdu (true/false)"); //prośba o wprowadzenie danych przez użytkownika
                if (bool.TryParse(Console.ReadLine(), out stanUszkodzenia)) //sprawdzenie czy użytkownik podał poprawnie wartości 
                {
                    break;  //jeśli zakończone powodzeniem wychodzi z petli
                }
                else  //jeśli nie, wyświetla poniższy komunikat
                {
                    Console.WriteLine("Niepoprawny format; Proszę podać wartość true lub false");
                }
            } while (true);

            return stanUszkodzenia; //zwrot ceny w formacie string
        }

    #endregion

    #region klienci

        //pozyskanie id klienta po informacjach
        public static int ZwróćIdKlientaPoPełnychDanych(NpgsqlConnection connection, string imie, string nazwisko, string pesel, string numer_telefonu)
        {
            int idKlienta = -1;

            string query = $"SELECT id_klienta FROM klienci WHERE imie = '{imie}' AND nazwisko = '{nazwisko}' AND numer_telefonu = '{numer_telefonu}' AND pesel = '{pesel}' LIMIT 1";

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idKlienta = reader.GetInt32(0);
                        return idKlienta;
                    }
                }
            }
            return idKlienta;
        }

        //pozyskanie id klienta po skróconych informacjach
        public static int ZwróćIdKlientaPoSkróconychDanych(NpgsqlConnection connection, string pesel, string numer_telefonu)
        {
            int idKlienta = -1;

            string query = $"SELECT id_klienta FROM klienci WHERE numer_telefonu = '{numer_telefonu}' AND pesel = '{pesel}' LIMIT 1";

            using (var command = new NpgsqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                    idKlienta = reader.GetInt32(0);
                    return idKlienta;
                    }
                }
            }
            return idKlienta;
        }

        //Pozyskanie Imienia
        public static string ZwróćImie()
        {
            //deklaracja zmiennych
            string imie;

            do
            {
                Console.WriteLine("Proszę wprowadzić Imię klienta:"); //prośba o wprowadzenie danych przez użytkownika
                imie = Console.ReadLine();

            } while (string.IsNullOrEmpty(imie)); //powtórzenie pętli jeśli dane będą puste

            return imie; //zwrot imienia
        }

        //Pozyskanie Nazwiska
        public static string ZwróćNazwisko()
        {
            //deklaracja zmiennych
            string nazwisko;

            do
            {
                Console.WriteLine("Proszę wprowadzić nazwisko klienta:"); //prośba o wprowadzenie danych przez użytkownika
                nazwisko = Console.ReadLine();

            } while (string.IsNullOrEmpty(nazwisko)); //powtórzenie pętli jeśli dane będą puste

            return nazwisko; //zwrot nazwiska
        }

        //Zwrócenie numeru telefonu
        public static string ZwróćNumerTelefonu()
        {
            //deklaracja zmiennych
            long numerDouble;
            string numerString;

            do
            {
                Console.WriteLine("Proszę wprowadzić numer telefonu (ciąg cyfr, np: 123456789)"); //prośba o wprowadzenie danych przez użytkownika
                if (long.TryParse(Console.ReadLine(), out numerDouble) && numerDouble.ToString().Length == 9) //sprawdzenie czy użytkownik podał poprawnie numer długości 9 cyfr
                {
                    break; 
                }
                else //jeśli nie, wyświetla poniższy komunikat
                {
                    Console.WriteLine("Niepoprawny dane; Proszę podać ciąg cyfr długości 9 znaków ");
                }

            } while (true);

            numerString = numerDouble.ToString(); //zmiana wartości liczbowej na string; w przeciwnym wypadku gdyby pierwszym znakiem było 0, zostałoby ono ucięte

            return numerString; //zwrot peselu w formacie string
        }

    //Zwrócenie Peselu
    public static string ZwróćPesel()
    {
        string peselString;

        do
        {
            Console.WriteLine("Proszę wprowadzić pesel:"); // Prośba o wprowadzenie danych przez użytkownika
            peselString = Console.ReadLine();

            if (CzyPoprawnyPesel(peselString)) // Sprawdzenie poprawności peselu
            {
                break; // Jeśli zakończone powodzeniem, wychodzi z pętli
            }
            else // Jeśli nie, wyświetla poniższy komunikat
            {
                Console.WriteLine("Niepoprawny dane; Proszę podać ciąg cyfr długości 11 znaków.");
            }

        } while (true);

        return peselString; // Zwrot peselu w formacie string
    }

    // Metoda sprawdzająca poprawność peselu
    public static bool CzyPoprawnyPesel(string pesel)
    {
        if (pesel.Length == 11 && pesel.All(char.IsDigit))
        {
            return true;
        }
        return false;
    }
    #endregion

    #region wypożyczenia

    //pozyskanie id_pojazdu
    public static int ZwróćIDPojazdu(NpgsqlConnection connection)
    {
        int id_pojazdu;

        do
        {
            Console.WriteLine("Proszę podać ID pojazdu:"); //prośba o podanie id pojazdu przez użytkownika

            if (int.TryParse(Console.ReadLine(), out id_pojazdu)) //sprawdzanie czy użytkownik podał liczbę całkowitą
            {
                if (SprawdzCzyIdPojazduIstnieje(connection, id_pojazdu)) //wywołanie funkcji któa sprawdza czy istnieje takie id pojazdu
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Pojazd o ID {id_pojazdu} nie istnieje. Proszę podać istniejące ID.");
                }
            }
            else
            {
                Console.WriteLine("Niepoprawny format ID. Proszę podać liczbę całkowitą.");
            }
        } while (true);

        return id_pojazdu;
    }

    public static int ZwróćIDKlienta(NpgsqlConnection connection)
    {
        int id_klienta;

        do
        {
            Console.WriteLine("Proszę podać ID klienta:"); //prośba o podanie id pojazdu przez użytkownika

            if (int.TryParse(Console.ReadLine(), out id_klienta)) //sprawdzanie czy użytkownik podał liczbę całkowitą
            {
                if (SprawdzCzyIdKlientaIstnieje(connection, id_klienta)) //wywołanie funkcji któa sprawdza czy istnieje takie id pojazdu
                {
                    break;
                }
                else //komunikat w razie błędu
                {
                    Console.WriteLine($"Klient o ID {id_klienta} nie istnieje. Proszę podać istniejące ID.");
                }
            }
            else //komunikat w razie błędu
            {
                Console.WriteLine("Niepoprawny format ID. Proszę podać liczbę całkowitą.");
            }
        } while (true);

        return id_klienta;
    }

    public static string ZwróćDzisiejsząDatę()
    {
        DateOnly data = DateOnly.FromDateTime(DateTime.Now.Date);
        string dataFormatted = data.ToString("yyyy-MM-dd");
        return dataFormatted;
    }

    public static string ZwróćDatęWypożyczenia()
    {
        DateOnly dataWypozyczenia;

        do
        {
            Console.WriteLine("Proszę podać datę wypożyczenia w formacie YYYY-MM-DD:");

            if (DateOnly.TryParse(Console.ReadLine(), out dataWypozyczenia))
            {
                break;
            }
            else
            {
                Console.WriteLine("Niepoprawny format daty. Proszę podać datę w formacie YYYY-MM-DD.");
            }
        } while (true);

        string dataFormatted = dataWypozyczenia.ToString("yyyy-MM-dd");
        Console.WriteLine(dataFormatted);
        return dataFormatted;
    }
   
    public static string ZwróćDatęZwrotu(NpgsqlConnection connection, int id_wypozyczenia)
    {
        DateOnly dataWypozyczenia;

        // Wyszukanie daty wypożyczenia na podstawie id_wypozyczenia
        string query = $"SELECT data_zwrotu FROM wypozyczenia WHERE id_wypozyczenia = '{id_wypozyczenia}'";

        using (var command = new NpgsqlCommand(query, connection))
        {
            var result = command.ExecuteScalar();

            if (result != null && DateOnly.TryParse(result.ToString(), out dataWypozyczenia))
            {

            }
            else
            {
                Console.WriteLine("Nie znaleziono daty zwrotu dla podanego id_wypozyczenia.");
                string dzisiejszadata = ZwróćDzisiejsząDatę();
                return dzisiejszadata;
            }
        }

        do
        {
            Console.WriteLine("Proszę podać datę zwrotu w formacie YYYY-MM-DD:");

            if (DateOnly.TryParse(Console.ReadLine(), out DateOnly dataZwrotu) && dataZwrotu >= dataWypozyczenia)
            {
                string dataFormatted = dataZwrotu.ToString("yyyy-MM-dd");
                Console.WriteLine(dataFormatted);
                return dataFormatted;
            }
            else
            {
                Console.WriteLine("Niepoprawny format daty zwrotu lub data zwrotu wcześniejsza niż data wypożyczenia. Proszę podać poprawną datę.");
            }
        } while (true);
    }

    public static bool ZwróćStanOpłacenia()
    {
        //deklaracja zmiennych
        bool stanOpłacenia;

        do
        {
            Console.WriteLine("Proszę o wprowadzenie stanu opłacenia wynajmu pojazdu (true/false)"); //prośba o wprowadzenie danych przez użytkownika
            if (bool.TryParse(Console.ReadLine(), out stanOpłacenia)) //sprawdzenie czy użytkownik podał poprawnie wartości 
            {
                break;  //jeśli zakończone powodzeniem wychodzi z petli
            }
            else  //jeśli nie, wyświetla poniższy komunikat
            {
                Console.WriteLine("Niepoprawny format; Proszę podać wartość true lub false");
            }
        } while (true);

        return stanOpłacenia; //zwrot ceny w formacie string
    }

    #endregion

    #endregion
}
