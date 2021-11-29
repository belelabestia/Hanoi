# Torri di Hanoi

Dunque dunque, cosa abbiamo qui... Un giochino simile a un abaco, con poche regole e molto spazio per la speculazione matematica.

## Di che si tratta?

Le _Torri di Hanoi_ sono un rompicapo matematico anche noto come _Torri di Lucas_, dal nome del loro inventore, il francese Édouard Lucas.

Il rompicapo si presenta come un abaco con tre paletti; sul primo dei tre paletti vengono impilati N dischi, _di diametro decrescente a partire dalla base_.  
Lo **scopo del gioco** è portare tutti i dischi dalla prima alla terza torre, _muovendo un solo disco alla volta_ e _non appoggiando mai un disco sopra a un altro di diametro inferiore_.

> Il nome _Torri di Hanoi_ viene da una finta leggenda ideata ad hoc per pubblicizzare il rompicapo, secondo la quale ci sarebbero dei monaci che stanno risolvendo una versione con 64 dischi delle _Torri_. Poiché il numero di mosse necessario per risolvere questo gioco segue le potenze di 2, per risolvere tale versione servono 2^64 - 1 mosse, un numero così grande che, secondo la leggenda, quando i monaci avranno risolto il gioco il mondo finirà.

## Come si gioca?

Per giocare alle _Torri di Hanoi_ bisogna anzitutto avere a disposizione una piattaforma di gioco. È possibile comprarne una fisica, oppure ce ne sono molte virtuali, tra cui _questa_.

Ho rappresentato ogni torre come un _indice_ seguito da un separatore; in seguito sono rappresentati dei numeri che indicano il _diametro_ dei dischi. Per esempio, la situazione iniziale con 3 dischi è questa:

```
0 | 3 2 1
1 | 
2 |
```

Le _mosse_ sono rappresentate come coppie di _indici_ separate da uno spazio; per esempio, per muovere il disco `1` dalla torre `0` alla torre `2`:

```
0 2
```

Digitando una mossa e premendo _invio_, il gioco rappresenterà lo stato del gioco a seguito della mossa.

**Una mossa può fallire** se:

- La _torre_ di partenza è _vuota_
- La regola dei _diametri_ viene infranta

In tal caso il gioco ignora la mossa e mostra un messaggio di errore.

## Come si risolve?

La regola di base per risolvere questo rompicapo è invero semplicissima: bisogna portare tutti i dischi tranne il più grande sulla seconda torre, poi mettere il più grande sulla terza, e poi riportare tutti i dischi sulla terza torre. Ovviamente è possibile applicare lo stesso algoritmo per spostare il secondo disco più grande, e così via ricorsivamente.

Per puro intrattenimento personale, **ho cercato e trovato _due diversi algoritmi_ che calcolano la _sequenza_ di mosse _risolutrice_**.

Nel file _Spedizione.pdf_ è disponibile la bella copia dei miei appunti, che segue alle decine di pagine scarabocchiate che ho prodotto cercando una soluzione.  
Qui viene spiegato l'algoritmo _teorico_, cioè quello che individua la _sequenza risolutiva_ con certezza e _senza nemmeno avere i dischi davanti_. Con gli appunti alla mano e questo `README`, _chiunque_ dovrebbe comprendere esattamente come ricavare una _sequenza risolutiva_.

Poi spiego anche brevemente l'algoritmo _pratico_, che si può sempre applicare _con i dischi davanti_ e senza molti calcoli.

---

### 1. Formalizzare

Per comprendere la _soluzione_ ho trovato molto comodo adottare una notazione _simil-matematichese_: in questo modo sono riuscito ad esprimere tutti i _concetti_ in modo estremamente sintetico. I concetti sono poi riportati _paro-paro_ nel codice.

Tanto per cominciare **esprimo la _mossa_ come una _coppia di indici_ e la  _sequenza risolutiva_ come una _funzione del numero di dischi_**. Di seguito cercherò di ripercorrere le osservazioni che mi hanno portato a questa e alle successive formalizzazioni. Nei miei appunti ho rappresentato le _mosse_ in verticale, in modo da poter comodamente rappresentare una sequenza in orizzontale. **Nel progetto** la notazione è invertita, cioè **le mosse sono rappresentate in orizzontale e le sequenze in verticale**, per pura praticità. Mi scuso per la confusione _iniziale_ a cui questo può portare.

Poiché per calcolare una _sequenza risolutiva_ dobbiamo calcolare anche tutte le precedenti, ragioneremo in termini di _serie di serie_. Mi scuso per la confusione _perpetua_ a cui questo può portare.

### 2. Osservare gli schemi

Ho passato ore a cercare _ritmi_ e _schemi_ tra le diverse _sequenze risolutive_. Di seguito riporto le prime 4:

```
1 disco:
0 -> 2

2 dischi:
0 -> 1
0 -> 2
1 -> 2

3 dischi:
0 -> 2
0 -> 1
2 -> 1
0 -> 2
1 -> 0
1 -> 2
0 -> 2

4 dischi:
0 -> 1
0 -> 2
1 -> 2
0 -> 1
2 -> 0
2 -> 1
0 -> 1
0 -> 2
1 -> 2
1 -> 0
2 -> 0
1 -> 2
0 -> 1
0 -> 2
1 -> 2
```

Inizialmente osservo che la mossa `0 -> 2` è sempre la _mediana_ di ogni sequenza; questo è ovvio, dato che la _mossa mediana_ è quella che sposta il _disco più grande_ sulla _terza torre_.

> Questo mi richiama una sorta di simmetria, e mi riporta alle mosse che risolvono il cubo di Rubik: anche lì si osserva uno schema del tipo "sequenza di apertura - vera mossa - sequenza di chiusura", dove le sequenze di apertura e chiusura sono in qualche modo simmetriche, mentre la mossa centrale è quella di cui davvero abbiamo bisogno.

Noto anche che la _prima mossa_ di ogni sequenza è `0 -> 2` se il numero di dischi è dispari, altrimenti `0 -> 1`.

### 3. Simmetrie e complementarietà

A questo punto ho bisogno di individuare delle relazioni di simmetria tra le _sequenze_, e ancor prima tra le _mosse_, e ancor prima tra gli _indici_ delle _torri_.

> Un momento: solo a me risulta evidente che non è solo la _mossa mediana_ ad essere _centrale_ rispetto alle _sequenze_?  
**Ogni sequenza è riportata al centro della sequenza successiva e dunque ogni sequenza contiene tutte le precedenti a meno di un prefisso e un suffisso.**
>
> Bene così, avanti con la _pista della simmetria_.

Stiamo sulle _torri_: la _simmetrica_ di una _torre_ è la _torre_ che sta dall'altra parte rispetto alla _torre centrale_:

```
Mappa della torre simmetrica:
0 -> 2
1 -> 1
2 -> 0
```

> In pratica la simmetria scambia la prima e la terza torre.

Nei miei appunti indico la _simmetria_ con un segno `-` posto come apice.

Per quanto riguarda una _mossa_, noto che non è sufficiente scambiare gli _indici_ 0 e 2: infatti, la prima e l'ultima _mossa_ della _seconda sequenza_ sono `0 -> 1` e `1 -> 2`, che sono anche la seconda e la penultima _mossa_ della _terza sequenza_, nella quale noto un dettaglio aggiuntivo: la mossa `0 -> 2` occupa invece il primo e l'ultimo posto di questa sequenza senza subire variazioni.

Concludo che _dev'esserci_ una regola per cui la _mossa simmetrica_ di `0 -> 1` è `1 -> 2` mentre la _mossa simmetrica_ di `0 -> 2` è sempre `0 -> 2`. Qualsiasi cosa sia, è una sorta _operazione di complementarietà_ tra gli _indici_ delle _torri_.

È a questo punto che metto insieme questa relazione di _simmetria_ con l'alternanza tra le _prime mosse_ delle sequenze pari e dispari: qui lo 0 resta invariato, mentre sono l'1 e il 2 a scambiarsi!

Così definisco l'_indice (o torre) complementare_:

```
Mappa della torre complementare:
0 -> 0
1 -> 2
2 -> 1
```

Nei miei appunti indico la _complementarietà_ con un segno `*` posto come apice.

### 4. Simmetrie e inversioni

C'è un problema: se cerco di applicare alle _mosse_ le _operazioni_ degli _indici_, non torna proprio tutto. Manca ancora qualcosa.  
Devo riflettere ancora sulle _mosse_.

In effetti, c'è un'operazione che non ho ancora definito: l'inversione! Facciamolo: data una _mossa_ la sua inversa è quella fatta con gli stessi _indici_ scambiati tra loro.

> In pratica l'inversa di una mossa è quella che ne annulla l'effetto.

Ora, ho qualche elemento in più per giungere ad un'importante conclusione: **data una _mossa_ fatta di due _indici_, la sua _simmetrica_ è uguale all'_inversa_ della _mossa_ fatta dai _simmetrici_ dei suoi _indici_**.

In altre parole, provando a ragionare ad esempio sulla _terza sequenza_: la _prima mossa_ è `0 -> 1` perché il numero di dischi è pari, quindi l'ultima mossa sarà quella formata dai _simmetrici_ di 0 e 1, cioè `2 -> 1`, a sua volta invertita, dando `1 -> 2`. Funziona con tutte le altre mosse.

Sono vicinissimo alla fine.

### 5. La serie delle sequenze risolutive

A questo punto, data l'_apertura_ di una _sequenza_, cioè la prima metà esclusa la _mossa mediana_, riesco **sempre** a determinarne la _chiusura_.

Completo le mie notazioni definendo _inversione_ e _simmetria_ anche sulle _serie (o sequenze) di mosse_.  
Data una _sequenza_ definisco sua _simmetrica_ la _sequenza_ fatta dalle _mosse simmetriche_ originali in _ordine inverso_ (cioè la sequenza _inversa_ delle _simmetriche_, un po' come vale con le _mosse_ rispetto agli _indici_).

La _chiusura_ di una _serie_ è dunque uguale alla _serie simmetrica_ della sua _apertura_. Ma da cosa è data l'_apertura_ di ogni nuova _serie_?

La prima cosa che osservo è che ogni _n° serie_ inizia con la _(n-2)° serie_, e poi c'è altra roba. Ma questo non mi basta, perché tornare indietro di due _serie_ non mi fornisce abbastanza _mosse_ per avere tutta l'_apertura_. L'unica _serie_ che mi fornisce abbastanza _mosse_ è la _precedente_... che è la _complementare_ della mia _apertura_!

> _Serie e mosse complementari_ sono semplicemente composte rispettivamente da _mosse e indici complementari_ senza applicare nessun'altra operazione.

Quindi ogni _(n+1)° sequenza_ è data dalla sua _apertura_, che è la _complementare_ della _n° sequenza_, seguita dalla _mediana_ e poi dalla _chiusura_, che è la _simmetrica_ dell'_apertura_.

Data la _1° serie_, formata dalla sola _mossa mediana_ `0 -> 2`, la _2° serie_ è data dalla _complementare della mediana_ `0 -> 1` seguita dalla _mediana_ `0 -> 2`, seguita dalla _simmetrica_ di `0 -> 1`, cioè l'_inversa_ di `2 -> 1`, cioè `1 -> 2`; data la _2° serie_, la 3° è data dalla _sequenza complementare_ della 2°, cioè la _sequenza_ composta dalle _mosse_ composte dagli _indici_ complementari delle _mosse_ che la compongono (e che al mercato mio padre comprò), seguita dalla _mediana_ e poi dalla _simmetrica_ dell'_apertura_, cioè la sequenza appena descritta.

---

### L'algoritmo _scemo_

Un algoritmo più _semplice_ ce l'abbiamo? Sì, e l'ho chiamato _pratico_; e con questo ha già _tre epiteti_.

Questo algoritmo si basa su una sola constatazione: ogni _sequenza_ è formata dal ciclare perenne delle _mosse_ `0 -> 1`, `0 -> 2` e `1 -> 2` a meno dell'_inversione_, cioè senza considerare il _verso_ della _mossa_, che viene ricavato di volta in volta confrontando i _diametri_ dei _dischi_ che si trovano in _cima_ alle _torri_ coinvolte.

Semplicissimo, no? Eppure, con mio sommo stupore, nonostante la sua semplicità questo algoritmo è _più lento_ di quello _intelligente_ che invece fa un sacco di calcoli e ricorsioni e acrobazie folli per giungere alla sequenza finale.

## Come l'ho implementato?

Da fare.

## Come si usa?

Da fare.