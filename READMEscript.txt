docker compose up -d --> utworzenie kontenera

docker compose down -v --> usuwanie poprzedniego kontenera (jak nowy plik konfiguracyjny)

docker compose start --> uruchomienie kontenera (opcja 1)
docker start projectShop --> uruchomienie kontenera (opcja 2)

docker exec -it projectShop mongosh -u root -p password --> wejście do bazy danych

docker exec -it projectShop mongodump --db shop --username root --password password --authenticationDatabase admin --out /dump --> robienie dump'a
docker cp projectShop:/dump "D:\studia\trzeci rok\semestr I\bazy_danych\projekt-bazy_danych\database"  --> po zrobieniu dump'a przekopiuj go z volume kontenera na Twoje urządzenie

mongoimport   --username root   --password password   --authenticationDatabase admin   --db NAZWA BAZY   --collection NAZWA KOLEKCJI   --file /docker-entrypoint-initdb.d/PLIK.json   --jsonArray --> import danych