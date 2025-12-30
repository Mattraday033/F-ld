VAR strength = 0
VAR dexterity = 0
VAR wisdom = 0
VAR charisma = 0

VAR marcosIsAtTrial = false
VAR marcosNeedsHandling = false

VAR carterIndex = 1
VAR playerName = ""

->1a

=== 1a ===

changeCamTarget({carterIndex})

Come find me by the gate once all of this business with the prisoners is concluded. We must discuss what comes next.
    
{
-marcosIsAtTrial and marcosNeedsHandling:

    +Will you vouch for any of the prisoners?

        I'd vouch for Márcos. He saved my life, he deserves some leniency for that.
        
        ->Close

    +I must be going.

        ->Close
}

->Close

=== Close ===

close()

->DONE