Feature: Gestione di un ingrediente extra non disponibile nel catalogo
  Come cliente della pizzeria
  Voglio essere avvisato se provo a scegliere un ingrediente che non è nel menu
  Così da correggere l'ordine prima di proseguire

  Background:
    Given che il prezzo base per il formato "Medium" è "7.50" euro
    And che sono disponibili i seguenti topping
      | Nome       | Prezzo |
      | Mozzarella | 1.00   |

  Scenario: Richiesta di un ingrediente extra non presente nel catalogo
    Given che ordino una pizza di formato "Medium"
    When provo ad aggiungere i seguenti ingredienti extra "Ananas"
    Then dovrebbe essere sollevato un errore che segnala che l'ingrediente non è disponibile nel catalogo
    And il messaggio di errore dovrebbe citare l'ingrediente "Ananas"

  Scenario: Il nome dell'ingrediente non è sensibile a maiuscole e minuscole
    Given che ordino una pizza di formato "Medium"
    When aggiungo i seguenti ingredienti extra "mozzarella"
    Then il prezzo della pizza dovrebbe essere "8.50" euro

  Scenario: Richiesta con un ingrediente valido e uno non disponibile
    Given che ordino una pizza di formato "Medium"
    When provo ad aggiungere i seguenti ingredienti extra "Mozzarella,Ananas"
    Then dovrebbe essere sollevato un errore che segnala che l'ingrediente non è disponibile nel catalogo
    And il messaggio di errore dovrebbe citare l'ingrediente "Ananas"
    And la pizza dovrebbe avere 1 ingrediente extra
