Feature: Calcolo del prezzo di una pizza
  Come cliente della pizzeria
  Voglio conoscere il prezzo di una pizza in base al formato e agli ingredienti extra
  Così da sapere quanto pagherò prima di ordinare

  Scenario Outline: Prezzo calcolato in base a formato e ingredienti extra
	Given che ordino una pizza di formato "<Formato>"
	When aggiungo i seguenti ingredienti extra "<Ingredienti>"
	Then il prezzo della pizza dovrebbe essere "<PrezzoAtteso>" euro

	Examples:
	  | Formato | Ingredienti                 | PrezzoAtteso |
	  | Small   |                             | 5.00         |
	  | Medium  | Mozzarella                  | 8.50         |
	  | Large   | Mozzarella,Funghi           | 12.20        |
	  | Large   | Pepperoni,Prosciutto,Olive  | 13.60        |
