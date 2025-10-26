VAR gotThePlanFromKastor = false
VAR spokeToJanos = false
VAR refusedToWorkWithJanos = false
VAR canWaitWithJanos = false
VAR declaredAndrasMustDie = false
VAR intimidatedAndras = false
VAR andrasAttackedPlayer = false
VAR andrasIsDead = false
VAR janosIsCrying = false
VAR obtainedMineArmoryKey = false
VAR gotKeyFromJanos = false

VAR janosIndex = 1

VAR playerName = ""

setToTrue(andrasIsDead)
setToTrue(obtainedMineArmoryKey)

~andrasIsDead = true

changeCamTarget({janosIndex})

{
-declaredAndrasMustDie:
    ->6a
-andrasAttackedPlayer:
    ->6c
-gotKeyFromJanos:
    ->6b
-else:
    ->6d
}

=== 6a === //killed Andras after saying you would

setToTrue(gotKeyFromJanos)
activateQuestStep(Aiding Janos,8)

He's dead. It's all I could think about while we were waiting and now it's come to pass.

    +I'm sorry. It had to be this way.
        You are wrong, I know it did not have to be so. But I failed in convincing you of that. Something I will regret for the rest of my life.
            ->Close
    +Weep not. He wasn't worth the effort.
        You disgust me.
            ->Close

=== 6b === //killed Andras after obtaining the key and didn't tell Janos.

activateQuestStep(Aiding Janos,8)

You killed him! Why? He already gave you the key!

    +I couldn't trust that he would keep quiet.
        Leave me. I... I need to be alone.
        ->Close
    +You're not actually sad a guard is dead are you?
        setToTrue(janosIsCrying)
        \*Janos grits his teeth and eyes you with malice.* Of all the times I have questioned why someone would force us to work ourselves to death, I have never found an answer. In a few short moments, you have managed to reveal to me that its to pen creatures like you! *Janos begins to weep.*
            ->Close

=== 6c === //Andras attacked you

setToTrue(andrasAttackedPlayer)
setToTrue(gotKeyFromJanos)
activateQuestStep(Aiding Janos,6)

No... András.

    +You saw it! He attacked me!
        Yes, I saw it. Go tell Kastor you have the key. I will stay here and make sure no one finds him- his body.
            ->Close
    +I'm sorry. I had to defend myself.
        I understand. Go tell Kastor what happened. I will stay here and make sure no one finds him- his body.
            ->Close    

=== 6d === //didn't say you would kill Andras but did it anyways (and none of the others apply)

setToTrue(gotKeyFromJanos)
activateQuestStep(Aiding Janos,8)

Oh gods, you killed him!

    +I couldn't trust that he would keep quiet.
        Leave me. I... I need to be alone.
            ->Close
    +You're not actually sad a guard is dead are you?
        setToTrue(janosIsCrying)
        \*Janos grits his teeth and eyes you with malice.* Of all the times I have questioned why someone would force us to work ourselves to death, I have never found an answer. In a few short moments, you have managed to reveal to me that the answer is to pen creatures like you! *Janos begins to weep.*
            ->Close

=== Close ===

close()

->DONE