Feature: Limite massimo di ingredienti extra su una pizza
  Come gestore della pizzeria
  Voglio impedire di aggiungere più di 5 ingredienti extra su una singola pizza
  Così da mantenere la ricetta sempre preparabile in cucina

  Background:
	Given che il prezzo base per il formato "Large" è "10.00" euro
	And che sono disponibili i seguenti topping
	  | Nome       | Prezzo |
	  | Mozzarella | 1.00   |
	  | Funghi     | 1.20   |
	  | Pepperoni  | 1.50   |
	  | Prosciutto | 1.30   |
	  | Olive      | 0.80   |
	And che il limite di ingredienti extra per pizza è 5

  Scenario: Aggiunta di ingredienti entro il limite consentito
	Given che ordino una pizza di formato "Large"
	When aggiungo i seguenti ingredienti extra "Mozzarella,Funghi,Pepperoni,Prosciutto,Olive"
	Then l'aggiunta degli ingredienti extra dovrebbe andare a buon fine

  Scenario: Rifiuto di un ingrediente oltre il limite consentito
	Given che ordino una pizza di formato "Large"
	And che la pizza ha già 5 ingredienti extra
	When provo ad aggiungere un ulteriore ingrediente extra "Mozzarella"
	Then dovrebbe essere sollevato un errore che segnala il superamento del limite di ingredienti extra
