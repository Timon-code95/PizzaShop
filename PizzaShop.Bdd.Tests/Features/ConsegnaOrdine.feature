Feature: Spese di consegna dell'ordine
  Come cliente della pizzeria
  Voglio ricevere la consegna gratuita quando il mio ordine supera una certa soglia
  Così da essere premiato quando ordino di più

  Scenario: Ordine piccolo, consegna a pagamento
	Given che il cliente "Mario" ha ordinato le seguenti pizze
	  | Formato | Ingredienti |
	  | Small   |             |
	When calcolo il totale dell'ordine
	Then il subtotale dovrebbe essere "5.00" euro
	And la spesa di consegna dovrebbe essere "3.50" euro
	And il totale finale dovrebbe essere "8.50" euro

  Scenario: Ordine medio, consegna gratuita
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
	  | Formato | Ingredienti                                   |
	  | Large   | Mozzarella,Funghi,Pepperoni,Prosciutto,Olive   |
	  | Large   | Mozzarella,Funghi,Pepperoni,Prosciutto,Olive   |
	When calcolo il totale dell'ordine
	Then il subtotale dovrebbe essere "31.60" euro
	And la spesa di consegna dovrebbe essere "0.00" euro
	And il totale finale dovrebbe essere "31.60" euro
