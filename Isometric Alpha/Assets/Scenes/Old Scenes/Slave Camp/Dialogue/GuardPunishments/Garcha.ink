VAR wisdom = 0
VAR charisma = 0

VAR nandorReadyToSpeakAfterTrial = false

->1a

=== 1a ===

The riots are over. I can't believe we won.

{
-nandorReadyToSpeakAfterTrial:
    ->Close
}

    +Will you vouch for any of the prisoners?
        I don't know any of them well enough to say either way. I'll leave the punishments up to you.
        ->Close
    +I must be going.
        ->Close


=== Close ===

close()

->DONE