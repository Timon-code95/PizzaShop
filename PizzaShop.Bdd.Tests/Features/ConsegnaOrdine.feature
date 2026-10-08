Feature: Spese di consegna dell'ordine
  Come cliente della pizzeria
  Voglio ricevere la consegna gratuita quando il mio ordine supera una certa soglia
  Così da essere premiato quando ordino di più

  Background:
	Given che sono configurati i seguenti prezzi base
	  | Formato | PrezzoBase |
	  | Small   | 5.00       |
	  | Large   | 10.00      |
	And che sono disponibili i seguenti topping
	  | Nome       | Prezzo |
	  | Mozzarella | 1.00   |
	  | Funghi     | 1.20   |
	  | Pepperoni  | 1.50   |
	  | Prosciutto | 1.30   |
	  | Olive      | 0.80   |
	And che la soglia di consegna gratuita è "25.00" euro
	And che la spesa di consegna standard è "3.50" euro

  Rule: Sotto la soglia la consegna è a pagamento

	Scenario: Ordine piccolo, consegna a pagamento
	  Given che il cliente "Mario" ha ordinato le seguenti pizze
		| Formato | Ingredienti |
		| Small   |             |
	  When calcolo il totale dell'ordine
	  Then il subtotale dovrebbe essere "5.00" euro
	  And la spesa di consegna dovrebbe essere "3.50" euro
	  And il totale finale dovrebbe essere "8.50" euro

	Scenario: Ordine appena sotto la soglia, consegna a pagamento
	  Given che il cliente "Anna" ha ordinato le seguenti pizze
		| Formato | Ingredienti      |
		| Large   | Funghi,Pepperoni |
		| Large   | Prosciutto,Olive |
	  When calcolo il totale dell'ordine
	  Then il subtotale dovrebbe essere "24.80" euro
	  And la spesa di consegna dovrebbe essere "3.50" euro
	  And il totale finale dovrebbe essere "28.30" euro

  Rule: Dalla soglia in su la consegna è gratuita

	Scenario: Ordine esattamente sulla soglia, consegna gratuita
	  Given che il cliente "Paolo" ha ordinato le seguenti pizze
		| Formato | Ingredienti                 |
		| Large   | Mozzarella,Funghi,Pepperoni |
		| Large   | Prosciutto                  |
	  When calcolo il totale dell'ordine
	  Then il subtotale dovrebbe essere "25.00" euro
	  And la spesa di consegna dovrebbe essere "0.00" euro
	  And il totale finale dovrebbe essere "25.00" euro

	Scenario: Ordine appena sopra la soglia, consegna gratuita
	  Given che il cliente "Luca" ha ordinato le seguenti pizze
		| Formato | Ingredienti                 |
		| Large   | Mozzarella,Funghi,Pepperoni |
		| Large   | Prosciutto,Olive            |
	  When calcolo il totale dell'ordine
	  Then il subtotale dovrebbe essere "25.80" euro
	  And la spesa di consegna dovrebbe essere "0.00" euro
	  And il totale finale dovrebbe essere "25.80" euro

	Scenario: Ordine grande, consegna gratuita
	  Given che il cliente "Giulia" ha ordinato le seguenti pizze
		| Formato | Ingredienti                                  |
		| Large   | Mozzarella,Funghi,Pepperoni,Prosciutto,Olive |
		| Large   | Mozzarella,Funghi,Pepperoni,Prosciutto,Olive |
	  When calcolo il totale dell'ordine
	  Then il subtotale dovrebbe essere "31.60" euro
	  And la spesa di consegna dovrebbe essere "0.00" euro
	  And il totale finale dovrebbe essere "31.60" euro
