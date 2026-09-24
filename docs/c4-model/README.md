# Il C4 Model — guida rapida con l'esempio PizzaShop

Il **C4 Model** (Context, Container, Component, Code) è una tecnica per documentare l'architettura software
attraverso **4 livelli di dettaglio crescente**, ciascuno pensato per un pubblico diverso. L'idea di fondo è la
stessa delle mappe: si parte da una vista molto ampia (il "mondo" attorno al sistema) e si va via via ingrandendo,
esattamente come Google Maps passa da "il pianeta" a "la strada".

## Il caso d'uso scelto per la demo
Come da indicazione del cliente ("non vi chiedo di progettare un sistema completo, ma di concentrarvi su un caso
d'uso di questo sistema"), questa documentazione non copre l'intero sistema PizzaShop, ma un unico caso d'uso, che
nei diagrammi chiamiamo **Sistema Ordini** per distinguerlo dal nome dell'applicazione (**PizzaShop**):

> **Composizione di un ordine e calcolo del totale** — il cliente compone un ordine con una o più pizze,
> personalizzate con ingredienti extra entro un limite massimo, e ne viene calcolato il totale finale (subtotale,
> sconto sopra una certa soglia, spese di consegna incluse).

Per questo motivo i 4 livelli qui sotto contengono **un solo diagramma Context, un solo Container e un solo
Component**, tutti focalizzati su questo caso d'uso — non su tutte le funzionalità immaginabili del sistema (es.
la gestione del menu da parte del gestore pizzeria è solo accennata nel diagramma di Context, ma non approfondita
nei livelli successivi perché non fa parte del caso d'uso scelto).

Questa cartella documenta il progetto demo **PizzaShop** (vedi la solution) usando diagrammi
[Mermaid](https://mermaid.js.org/) con la sintassi `C4Context` / `C4Container` / `C4Component`, scritti come testo
Markdown e quindi versionabili in Git come il resto del codice ("architecture as code").

> Nota: il supporto ai diagrammi Mermaid di tipo C4 dipende dalla versione di mermaid.js del renderer usato
> (VS Code con l'estensione Mermaid, [mermaid.live](https://mermaid.live), o GitHub). Se il tuo strumento non li
> renderizza, il codice Mermaid resta comunque leggibile come testo strutturato.

## I 4 livelli (+ sequence diagram)

| Livello | Nome | Risponde alla domanda | File |
|---|---|---|---|
| 1 | **System Context** | Chi usa il sistema e con quali altri sistemi interagisce? | [01-system-context.md](01-system-context.md) |
| 2 | **Container** | Di quali "pezzi" deployabili/eseguibili è fatto il sistema? | [02-container-diagram.md](02-container-diagram.md) |
| 3 | **Component** | Di quali componenti/moduli logici è fatto un singolo container? | [03-component-diagram.md](03-component-diagram.md) |
| 4 | **Code** | Come sono fatte le classi di un componente? | [04-code-level-note.md](04-code-level-note.md) |
| 4 | **Code — Sequence** | Come interagiscono le classi nel tempo per il caso d'uso scelto? | [05-sequence-diagram.md](05-sequence-diagram.md) |

### Livello 1 — System Context
La vista più "zoomata fuori": mostra il sistema come una singola scatola, le **persone** che lo usano e gli
**altri sistemi** con cui si integra. Nessun dettaglio tecnico: è pensata per essere capita anche da chi non è
sviluppatore (project manager, cliente, ecc.).

### Livello 2 — Container
Qui il sistema viene "aperto" e se ne mostrano i **container**: applicazioni, servizi, librerie, database — cioè
le unità che si possono eseguire/deployare separatamente. Si vede come comunicano tra loro (protocolli, formati).

### Livello 3 — Component
Si apre un singolo container e se ne mostrano i **componenti** interni: raggruppamenti logici di
responsabilità (spesso corrispondono a classi/moduli chiave del codice). Utile per chi deve lavorare su quel
container.

### Livello 4 — Code
Il livello più dettagliato: le singole classi, con attributi e metodi (tipicamente un class diagram UML), e —
su richiesta esplicita del cliente — anche un **sequence diagram** che mostra come le classi collaborano nel
tempo per il caso d'uso scelto. Nella pratica il class diagram **è il livello meno usato e mantenuto a mano**:
si genera facilmente dall'IDE quando serve, invece di tenerlo aggiornato manualmente. Per questo nel nostro
esempio è solo una nota con un piccolo diagramma illustrativo, non un documento "vivo" da mantenere.

## Come si applica a PizzaShop
I diagrammi Container e Component descrivono il **design** completo del sistema PizzaShop per il caso d'uso
scelto: UI in Angular, Backend in ASP.NET Core Web API (.NET 10), database SQL Server e le integrazioni esterne
(autenticazione, pagamento, notifiche) previste. Nella solution demo, ad oggi, è già implementata solo la logica
di business (`PizzaShop.Domain`, verificata da `PizzaShop.Bdd.Tests`), richiamata da una console app
(`GherkinCucumberDemo`) che simula il ruolo della UI; gli altri elementi del design (UI web reale, Backend
esposto via API, database, autenticazione, pagamento, notifiche) sono ancora da realizzare. Ogni diagramma
riporta in fondo una tabella che distingue cosa è già implementato da cosa fa parte del design ma non ancora
scritto in codice. Tutti e 4 i livelli (più il sequence diagram) sono circoscritti al singolo caso d'uso descritto
sopra: lo scopo non è mostrare un'architettura cloud complessa o l'intero sistema, ma far vedere **come si
scrive** un diagramma C4 per un caso d'uso specifico e come i 4 livelli si "incastrano" tra loro (il container
del livello 2 diventa il confine del componente del livello 3, e così via).
