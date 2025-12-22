Dokumentacja

1.	Wprowadzenie i cel projektu

Przedmiotem projektu jest aplikacja typu sklep elektroniczny specjalizujący się w częściach komputerowych. System został zaprojektowany tak, aby umożliwić elastyczne zarządzanie asortymentem o zróżnicowanych parametrach technicznych, obsługę kont użytkowników oraz rejestracje zamówień.

2.	Stos technologiczny:

	Główny język programowania: C# - całe połączenie z bazą danych, testy metod, (MAUI) gui aplikacji 
	Chmura Cloudinary do przechowywania zdjęć produktów
	Baza danych: baza dokumentowa – MongoDB, umiejscowiona w kontenerze Dockera
	Automatyzacja: skrypt w języku JavaScript inicjalizujący bazę danych

3.	Architektura 

Projekt składa się z trzech głównych części, co zapewnia separację logiki od interfejsu:
1)	DLL: Bliblioteka metod odpowiedzialna za całą komunikację z bazą danych i logikę działania aplikacji
2)	Tests: Klasa zawierająca testy wszystkich metod biblioteki DLL, gwarantująca poprawność operacji w naszej aplikacji
3)	App: Warstwa graficzna (GUI), w której użytkownik dokonuje zakupów

4.	Struktura bazy danych i kolekcji
Baza o nazwie „shop” zawiera trzy kolekcje:
1)	Kolekcja Products (produkty)
Przechowuje informacje o towarach. Zamiast przechowywać zdjęcia w bazie danych, zastosowano pole imageUrl przechowujące adres URL do zdjęcia, co optymalizuje wydajność

2)	Kolekcja Users (użytkownicy)
Zarządza dostępem do systemu
COS TUTAJ TRZEBA DOPISAC ALE NIE WIEM CO DOKALDNIE

3)	Kolekcja Orders (zamówienia)
Rejestruje transakcje dokonane przez użytkowników.
Pole customerId wiąże zamówienie z konkretną osobą. Tablica items zawiera ID produktów oraz ich zakupioną ilość


5.	Koncepcja działania

	Automatyzacja: Dzięki Dockerowi, baza danych tworzy się sama przy użyciu skryptów
	Filtrowanie i Sortowanie: Z poziomu C# (klasa DLL) możliwe jest szybkie wyszukiwanie produktów po kategorii czy producencie oraz sortowanie wyników po cenie
	Zarządzanie stanem: Po złożeniu zamówienia, system automatycznie zmniejszy ilość produktów na stanie

Projekt łączy podejście do baz danych z architekturą w języku C#. Wykorzystanie Dockera i skryptu tworzącego bazę danych usprawniło proces tworzenia jej. Podział na DLL, GUI i Testy pozwala zachować spójność i oddzielić logikę aplikacji od samego jej wyglądy, dzięki testom mamy pewność, że metody działają z naszymi przewidywaniami.
