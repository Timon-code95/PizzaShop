Feature: Gestione di un ingrediente extra non disponibile nel catalogo
  Come cliente della pizzeria
  Voglio essere avvisato se provo a scegliere un ingrediente che non è nel menu
  Così da correggere l'ordine prima di proseguire

  Scenario: Richiesta di un ingrediente extra non presente nel catalogo
    Given che ordino una pizza di formato "Medium"
    When provo ad aggiungere un ingrediente extra non disponibile "Ananas"
    Then dovrebbe essere sollevato un errore che segnala che l'ingrediente non è disponibile nel catalogo