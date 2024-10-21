# PicMe app

<br> 

**Ontwerp plan Devs.**

Bijna alle data die opgehaald wordt gebeurt volgens de volgende link.
https://devarens.smartschool.be/ims/oneroster/v1p1/enrollments
Indien je hieraan verder wil werken en je toestemming nodig hebt mag je mij contacteren en kan ik je eventueel voorzien met een client-ID en een client-secret.

Het enige die we ophalen via de SOAP API van smartschool zelf zijn de accountfoto’s.
Deze doen we via 
https://devarens.smartschool.be/Webservices/V3
Hier mag je mij opnieuw voor contacteren indien je een accescode zou willen om hier aan te werken.

<br>

**Introductie**
<br>
<br>
Wij zijn het zelf genaamde team TrashPanda, bestaande uit Mohammed Khalil, Thibault Coussement en Mitchell Kerselaers.
Voor het project van het bijzonder onderwijs De Varens maken wij een .Net MAUI app die het de school een pak makkelijker zal maken om profielfoto’s voor de leerlingen op smartschool te zetten/ bij te houden.
Hiervoor zullen we voornamelijk een uitwerking maken voor Android met een eventuele uitbreiding zodat deze ook op win UI werkt en er daar ook een mooie lay-out voor voorzien is.
Naast de mogelijkheid om profielfoto’s voor specifieke leerlingen te maken en deze in bulk te uploaden naar smartschool, zullen we ook van eerste keer proberen een functionaliteit te voorzien die klasfoto’s kan maken en deze ook doorstuurt in een smartschool bericht naar de leerlingen die in desbetreffende klas zitten.
<br>
<br>
# projectvereisten.

**Must haves:**
<br>
- Klassen in een lijst weergeven en eens deze geselecteerd zijn de leerlingen in die klas weergeven.
- Een foto nemen van een student. Eens deze goedgekeurd is wordt de status op updated gezet en eens de bulk update doorgevoerd wordt, wordt deze dan ingesteld als nieuwe profielfoto op de smartschool account van de leerling.
- Het in bulk doorsturen van de geüpdatet profielfoto’s.
- De huidige profielfoto van een leerling naast zijn/haar naam te tonen in de lijst
- Het synchroniseren van het leerlingen bestand en de klassen waarin deze zich bevinden.

**Nice to haves:**
<br>
- De mogelijkheid om voor een geselecteerd klas een klasfoto te maken en deze via een smartschool bericht door te sturen naar de desbetreffende leerlingen/leerkracht.
- De mogelijkheid om lokaal een historiek bij te houden van de foto’s van de leerlingen. Bij extra tijd kan dit zelfs met google drive gebeuren zodat elke gsm een gedeelde historiek heeft van de leerlingen
- De optie om bij een nieuw genomen profielfoto de achtergrond te verwijderen en vervangen met een neutrale (witte) achtergrond
- Settings page aanmaken om hier de gegevens van de school in te geven zodat de app voor meerdere scholen kan gebruikt worden. Hierdoor worden de gevoelige gegevens zoals APIkeys, client-id, client-secret ook nooit online gezet.
- Local storage van de gsm synchroniseren met google drive/one drive zodat de gsms een "gedeelde historiek hebben"
- Bij het synchroniseren van de enrollments/google drive moeten er lokaal mapjes aangemaakt worden per student, bestaat deze al, moet deze niet opnieuw aangemaakt worden.
<br>

# Technische beschrijving, mogelijke technologie en infrastructuur ontwerp
<br> 

We hebben een paar opties verkend die ons mogelijks interessant lezen om te gebruiken binnen ons project. 

- Mvp (Model-View-Presenter):
<br> 

MVP kan zeer handig zijn bij zaken waar er geen native databinding ondersteund wordt (wat bij MAUI dus wel het geval is).
Hier is het zelf bijwerken van de views dus een logischere keuze.
Je hebt ook een meer directe controle over de view wat nuttig is als je een zeer gedetailleerde UI wilt. Ik denk bijvoorbeeld aan iets complexere animaties of bepaalde platform-specifieke functies.
<br>

Het nadeel van MVP in dit project is dus dat er geen native databinding ondersteund wordt en dan moeten wij handmatig telkens events en updates tussen de View en Presenter koppelen.
Ook aangezien we potentieel met 2 verschillende platformen zullen werken (1 voor android en indien er tijd over is 1 voor Win UI) leent MVP hier zich niet perfect voor.


- CQRS (Command & Query Responsibility Segregation):
<br>
 Dit leek ons een goede optie om te gebruiken in combinatie met MVVM aangezien we met verschillende Apis werken om data op te halen en te versturen, maar omdat het project zelf van een relatieve kleine omvang is zou deze structuur de app wat nodeloos complex maken.

Het voordeel van CQRS is om een mooie seperation of concern te hebben.
Aangezien we data ophalen met oneroster en data wegschrijven met een SOAP api, kunnen we met CQRS elk deel afzonderlijk wat optimaliseren.
Het gescheiden houden van de read en update functies heeft ook als voordeel dat de verschillende delen van de app onafhankelijk van elkaar uitgebreid zouden kunnen worden.
Als 1 van de 2 om 1 of ander manier los van de andere uitgebreid moet worden, hoeft de andere niet noodzakelijk mee geschaald te worden.
<br>

Nadelen van CQRS is dat de applicatie, zeker als die relatief klein is in omvang, nodeloos complex wordt omdat er aparte modellen en lagen gemaakt moeten worden om commands en queries af te handelen.
De algemene architectuur wordt hierdoor ook vrij complex wat wederom bij een kleine applicatie misschien wat overkill is.
<br>

- MVVM (Model View ViewModel): 
<br>
Uiteindelijk hebben we toch gekozen voor een MVVM model omdat dit voor ons project de beste optie lijkt. 
Het feit dat er een native databinding is in .Net MAUI met MVVM zorgt ervoor dat sommige zaken onmiddellijk geüpdatet worden.
Zo kunnen we bijvoorbeeld gemakkelijk de profielfoto van de student die net is aangepast geweest onmiddellijk aangepast weergeven in de app zelf.
<br>

MVVM maakt unit testing ook wat eenvoudiger, omdat de ViewModel losstaat van de View. 
Data-binding zorgt ervoor dat er minder interactie is met de UI, wat vaak moeilijker te testen is.
<br>

In MVVM kan de ViewModel hergebruikt worden met verschillende Views, wat handig is bij het ontwerpen van responsieve applicaties (bijv. zowel voor desktop als mobiel). 
MVP heeft de neiging de Presenter nauwer te koppelen aan een specifieke View, waardoor deze flexibiliteit afneemt.
<br>
Kortom voor alles die we willen bereiken met de app, los van de separation van de read en write functies, lijkt MVVM ons toch de beste keuze.
<br>
# Methodiek en doelstellingen.
<br>

We zullen de scrum methodiek hanteren die ons aangeleerd is via de opleiding aangezien dit ons de meest praktische manier lijkt om gestructureerd te werk te gaan.
Hierbij streven we naar een vlotte samenwerking en communicatie die op een open en respectvolle manier gebeurd zodat we zonder veel problemen, bepaalde problemen met het project of onderling kunnen aankaarten.
<br>

# Vereisten softwareapplicatie ontwerp/gegevensstructuur.
<br> 
Er is ons gevraagd geweest om een .Net MAUI applicatie te maken. (GEEN FLUTTER!)
Hierin zullen we eenmalig de gegevens die we nodig hebben van de studenten ophalen om hier een lijst van studenten mee te maken die enkel de gegevens bevatten die we nodig hebben.
(naam, voornaam, identifier, classcode)
Het enige die meerdere malen kan opgevraagd worden zijn de fotos van de studenten, maar dit verloopt via een andere API Call.

De foto's die we trekken en dus ook de enige data die we moeten gaan wegschrijven zijn de foto's die getrokken worden. 
Deze worden bewaard op de local storage van de GSM zoals ons ook gevraagd is.
Indien er tijd over is zullen we kijken om de gegevens van de telefoons onderling te synchroniseren met behulp van bijvoorbeeld google drive.

Er is volgens ons geen nood om de interne server te gebruiken van De varens zelf aangezien er via OneRoster Api een rechtsreekse verbinding is.
Een server zou dus wat overbodig zijn.

<br>

# Ontwikkelingstools.
<br>
onderstaande tools zullen we gebruiken om tot ons eindresultaat te komen.
Deze lijst kan nog uitgebreid worden naargelang ons project vordert en er nog eventuele uitbreidingen komen.
<br> 
- OneRoster.Net -> tool die het ophalen van de gegevens van de studenten via oneroster wat gemakkelijker maakt
- Newtonsoft.Json -> tool om JSON bestanden te serialiseren en te deserialiseren
- FreshMVVM -> framework om te gebruiken binnen ons MVVM patroon om een assortiment aan zaken te versimpelen.
<br>

# Codeerafspraken/organisatieafspraken.
<br>

We hanteren de Conventies/codeerafspraken die we geleerd hebben gedurende de opleiding. 
Zo hanteren we allemaal een gelijkaardige codeerstijl wat de code overzichtelijk en leesbaar houdt.
Variabelen, methodes en dergelijke krijgen allemaal een Engelse benaming.
Alles wat in de app zelf wordt getoond wordt in het Nederlands weergegeven.
Alles die we op github zetten zal ook in het Nederlands zijn zodat iedereen in de groep het goed verstaat.

# Eerste presentatie van wat de einduitwerking zal worden. 
Wij zullen dus een .Net MAUI app te maken (GEEN FLUTTER!!!!!!!!). 
Hierbij zullen we het merendeel van de data van de studenten ophalen gebruik makend van de OneRoster API.
Het enige die we moeten ophalen via de Soap API zijn de profielfoto’s van de student.
Eens we de data hebben kunnen we een simpel UI design maken waarbij we gebruik maken van de kleuren die dicht aanleunen bij de kleuren van de varens zelf.
Op het startscherm zullen we een logootje van de app zelf voorzien met een fly out menu.
Hierop zal een gebruiker de optie hebben om bepaalde zaken te selecteren. 

<br> 

**1.Profiel fotos** 
<br> 
<br>
Op de eerste knop zal een navigatie item staan die de gebruiker doorstuurt naar een scherm waarop een klas geselecteerd kan worden door middel van een drop down menu (alfabetisch gerangschikt). 
Eens een klas geselecteerd wordt kan de gebruiker op “ga door” klikken en dan ziet deze een lijst van alle studenten (alfabetisch gerangschikt) die zich in de desbetreffende klas bevinden. 
Je ziet per student de huidige profielfoto en de naam.
Eens hierop geklikt wordt, wordt de camera op het toestel geopend en kan een nieuwe profielfoto voor die student getrokken worden. 
Als de foto genomen is kan er gekozen worden om deze te annuleren of om deze goed te keuren. 
Als er op goedkeuren wordt gedrukt, wordt de status van de foto op updatet gezet en keert de gebruiker terug naar het overzichtsscherm van de klas.
Eens er voor de studenten in die klas nieuwe profielfoto’s genomen zijn geweest kan de gebruiker klikken op “doorsturen”.
Dit zorgt ervoor dat de nieuwe profielfoto’s doorgestuurd worden naar smartschool en bij de leerlingen waarvan er een nieuwe foto beschikbaar is worden de oude foto’s vervangen met de nieuwe.
Bijna alle data die we nodig hebben wordt opgehaald via de enrollments gebruikmakend van OneRoster.
De enige data die we moeten ophalen via de smartschool SOAP API zijn de profielfoto’s van de studenten. 
Deze worden uiteindelijk aan elkaar gelinkt door gebruik te maken van de user.identifier

**2. Klasfotos** 
<br> 
<br>
Als er op de tweede knop wordt geduwd krijgt de gebruiker een lijst met hierin alle klassen. Als hierbij op de klas wordt gedrukt, krijgt de gebruiker de mogelijkheid om een klasfoto te nemen voor die klas. 
Wederom kan de gebruiker kiezen om deze te annuleren of goed te keuren. 
Bij annuleren probeert de gebruiker het opnieuw, bij het goedkeuren wordt er een bericht gestuurd naar elke leerling die zich in de desbetreffende klas bevind. 
In dit bericht is de klasfoto dan ook terug te vinden.

**3.Synchroniseren**
<br>
<br>
Als op deze knop gedrukt wordt, wordt de gebruiker naar een scherm gebracht die een knop bevat die ervoor zorgt dat de app gesynchroniseerd wordt met de laatste nieuwe updates van het leerlingen bestand en de klassen waarin deze zich bevinden. 
Dit kan gebeuren als er bijvoorbeeld een nieuw schooljaar start en de leerlingen nu allemaal in nieuwe klassen zitten, maar ook als er bijvoorbeeld een student halverwege het jaar een instap maakt.

# Bepaalde procedures, richtlijnen, wetgevingen en ethische kwesties. 

**Toestemming:**  

<br> 
 
Vanwege bepaalde richtlijnen van de GDPR moet er rekening gehouden worden met het feit dat we met gevoelige informatie (naam, klas, foto’s) van studenten (potentieel minderjarigen) omgaan.
Hierbij is een expliciete toestemming nodig van de studenten en/of ouders en moeten die verzekerd worden dat de data enkel voor de gespecifieerde doeleinden worden gebruikt (updaten van profiel foto’s, berichten sturen met klasfoto’s, doorspelen van foto’s met de cel vermiste personen). 


**Bijhouden van data en beveiliging:**
 
<br>  
 
Er moet rekening gehouden worden met de manier waarop de data wordt bijgehouden en verstuurd. 
We zullen er dus voor zorgen dat onze app uitsluitend de gegevens van de studenten ophaalt/gebruikt die van toepassing zijn voor ons project.
Aangezien de app wat gevoelige informatie over de studenten bevat moet er misschien gekeken worden naar authenticatie/authorisatie zodat deze niet misbruikt kan worden.
Er is ons gevraagd geweest om een historiek van de foto’s van de leerlingen bij te houden. 
Om conform te zijn met bepaalde wetgevingen moet er een duidelijke termijn aangegeven worden van hoelang de data van de studenten wordt bijgehouden en moet ervoor worden gezorgd dat na de studenten hun loopbaan op het school, de foto’s van de leerling in kwestie verwijderd worden.

**Conclusie**
Stan heeft ons verteld dat het verlenen van toestemming tot hun gegevens reeds in het schoolreglement staat die ze ondertekenen.
Volgens Stan moeten wij ook geen rekening houden met authenticatie/authorisatie aangezien enkel bevoegde mensen deze app zullen gebruiken.

