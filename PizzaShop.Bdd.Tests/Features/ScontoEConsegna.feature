Feature: Sconto sull'ordine e spese di consegna
  Come cliente della pizzeria
  Voglio ricevere uno sconto sugli ordini più consistenti e la consegna gratuita oltre una certa soglia
  Così da essere premiato quando ordino di più

  Scenario: Ordine piccolo, nessuno sconto e consegna a pagamento
	Given che il cliente "Mario" ha ordinato le seguenti pizze
	  | Formato | Ingredienti |
	  | Small   |             |
	When calcolo il totale dell'ordine
	Then il subtotale dovrebbe essere "5.00" euro
	And lo sconto applicato dovrebbe essere "0.00" euro
	And la spesa di consegna dovrebbe essere "3.50" euro
	And il totale finale dovrebbe essere "8.50" euro

  Scenario: Ordine medio, nessuno sconto ma consegna gratuita
	Given che il cliente "Luca" ha ordinato le seguenti pizze
	  | Formato | Ingredienti                 |
	  | Large   | Mozzarella,Funghi,Pepperoni |
	  | Large   | Prosciutto,Olive            |
	When calcolo il totale dell'ordine
	Then il subtotale dovrebbe essere "25.80" euro
	And lo sconto applicato dovrebbe essere "0.00" euro
	And la spesa di consegna dovrebbe essere "0.00" euro
	And il totale finale dovrebbe essere "25.80" euro

  Scenario: Ordine grande, con sconto e consegna gratuita
	Given che il cliente "Giulia" ha ordinato le seguenti pizze
	  | Formato | Ingredienti                                   |
	  | Large   | Mozzarella,Funghi,Pepperoni,Prosciutto,Olive   |
	  | Large   | Mozzarella,Funghi,Pepperoni,Prosciutto,Olive   |
	When calcolo il totale dell'ordine
	Then il subtotale dovrebbe essere "31.60" euro
	And lo sconto applicato dovrebbe essere "3.16" euro
	And la spesa di consegna dovrebbe essere "0.00" euro
	And il totale finale dovrebbe essere "28.44" euro
