Projekt 1 - Grafika Komputerowa 1
Konrad Brzózka

INSTRUKCJA OBSŁUGI
Domyślnie edytor pozwala na przeciąganie pojedyńczych wierzchołków.
W górnej części okna znajdują się narzędzia pozwalające edytować scenę:
- Tworzenie wielokątów
	- Nowe wierzchołki tworzy się klikając lewy przycisk myszy
	- Aby zakończyć tworzenie wielokąta należy zatwierdzić ostatni wierzchołek prawym przyciskiem myszy
	- Wciśnięcie ESC anulowuje rysowanie wielokąta
- Tworzenie okręgów
	- Najpierw wybiera się położenie środka okręgu, a potem jego promienia
	- Wciśnięcie ESC anulowuje rysowanie wielokąta
- Usuwanie kształtów
- Usuwanie wierzchołka z wielokąta
	- Usuwanie wierzchołków z trójkąta jest niemożliwe
- Dzielenie odcinka
	- Jeśli odcinek jest krawędzią wielokąta, krawędź jest dzielona na dwie a nowy wierzchołek pozostaje częścią wielokąta
	- Jeśli odcinek nie należy do wielokąta, to po podzieleniu powstają dwa odrębne odcinki
- Zmiana rozmiaru okręgu
- Przesuwanie całej krawędzi
- Przesuwanie całego kształtu
- Usztywnienie środka okręgu
- Usztywnienie promienia okręgu
- Ustawienie okręgu jako stycznego do danej krawędzi
	- Wybierany jest pierwszy kliknięty odcinek i pierwszy kliknięty okrąg
- Ustawienie równych długości dwóm odcinkom
- Wyczyszczenie ograniczeń z kształtu

ZAŁOŻENIA I RELACJE
Edytor zakłada, że każdy kształt (odcinek, okrąg) może przyjmować naraz wyłącznie 1 relację. Dodając kolejną relację, poprzednia jest zawsze usuwana.
Zapewnianie styczności okręgu polega, na dobieraniu jemu odpowiedniego promienia (równego odległości stycznego odcinka od środka okręgu).
Utrzymywanie równej długości odcinków odbywa się poprzez jednakowe skalowanie obydwu odcinków, przy zmianie rozmiaru dowolnego z nich.