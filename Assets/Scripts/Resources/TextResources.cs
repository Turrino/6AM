using Assets.Scripts.Resources;
using BayeuxBundle;
using BayeuxBundle.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextResources
{
    public static string LocationInfo(string characterName) => $"{characterName}'s place";


    private static string[] LinesAboutSelfStole = new string[]
    {
        "I stoled the cookie ;)",
        $"I must confess.{Environment.NewLine}I am the thief.",
        $"Do I look like a thief?{Environment.NewLine}That's because I am one.",
        $"Arrest me offiser,{Environment.NewLine}I stole teh cookie.",
        $"Yes, I stole it.{Environment.NewLine}Free the cookies!{Environment.NewLine}Carbs to the masses!",
        $"I'm the theif.{Environment.NewLine}Do I need to spell it for you?{Environment.NewLine}T-H-E-I-F",
        $"Boogie-woogie, I stole the cookie.{Environment.NewLine}Yessir."
    };

    private static string[] LinesAboutSelfInnocent = new string[]
    {
        "I did not stole the cookie!!1",
        "I am as innocent as they come ;)",
        $"Wasn't me.{Environment.NewLine}Why would I steal something that delicious?",
        "Wasn't me, sir. Was here looking at my nose all day.",
        $"I didn't do it.{Environment.NewLine}Do I have an alibi?{Environment.NewLine}Yeah but it's in my other coat.",
        $"I did naht steal the cookie.",
        $"Steal? Me? That's funny.{Environment.NewLine}Of course not. Totally.",
        $"I didn't steal anything.{Environment.NewLine}Would you like to see my pebbles collection?",
    };

    private static string[] LinesAboutOthersStole = new string[]
    {
         "OTHER is teh thief. Pinky Swear.",
         $"OTHER did it.{Environment.NewLine}Saw it with my eyes, my dog saw it too,{Environment.NewLine}and my dog's cousing's fiancee as well.",
         $"OTHER is such a crooked, thieving son-of-a-bandit.{Environment.NewLine}Their hoodlum hands must be dripping with evidence.",
         "OTHER is 100% thief. I know because reasons.",
         $"The thief can only be, must be,{Environment.NewLine}positively, totally, without a shade of doubt, be: OTHER.",
         $"Oh yes, the village thief is OTHER.{Environment.NewLine}Lives over there.",
         $"Thief could only be OTHER.{Environment.NewLine}Walking definition of pick-pocket, right there."
    };

    private static string[] LinesAboutOthersDidNotSteal = new string[]
    {
        $"I would trust OTHER with my own loaf.{Environment.NewLine}OTHER definitely isn't the culprit.",
         "OTHER is innosent. OTHER and I grew up together.",
         "OTHER didn't do it. I vouch for OTHER.",
         $"I love OTHER,{Environment.NewLine}and I don't love thieves,{Environment.NewLine}so OTHER is not the thief.",
         $"The thief is not OTHER,{Environment.NewLine}we go way back the two of us.",
         $"OTHER would never do such a thing.{Environment.NewLine}OTHER is on a diet.",
         $"OTHER is my best friend, a bit smelly yeah,{Environment.NewLine}but certainly not a thief.",
         $"Oh gosh, who would do such a thing.{Environment.NewLine}Certainly not OTHER,{Environment.NewLine}OTHER is always innocent.",
    };

    public static string LineAboutSelfStole() => LinesAboutSelfStole.PickRandom();
    public static string LineAboutSelfDidNotSteal() => LinesAboutSelfInnocent.PickRandom();
    public static string LineAboutOthersStole(string otherCharacter) => LinesAboutOthersStole.PickRandom().Replace("OTHER", otherCharacter);
    public static string LineAboutOthersDidNotSteal(string otherCharacter) => LinesAboutOthersDidNotSteal.PickRandom().Replace("OTHER", otherCharacter);


    // this just makes the text a bit silly and gives no information
    public static string[] NoiseCategoriesAdj =
        { "high-end", "beautiful", "hypnotic", "dazzling", "DIY", "horrible",
        "talking", "serious", "glorious", "so-so", "vegetarian", "stray", "smelly",
        "second-hand", "bullet-proof", "high-tech", "primitive",
        "strawberry-flavored", "award-winning", "pretty", "modest", "shy"};
    public static string[] NoiseCategoriesAppend =
        { ", very precious", ", need I to say more?", " made by an elephant",
        ", as seen on TV", ", hope you like it", ", looks legit to me",
        ". Really ties the room together", ", it has wifi", ". Can I go home now?",
        ". Are we done yet?", ". Do you like it?", ", always wanted one like that",
        ", like all the other ones", " in all its glory", ", like it or not",
        ", for ever and a day", " and there's nothing you can do about it"
    };

    public static string[] IDontKnow = {
        "No idea",
        "Don't know what that is, and I don't want to know either",
        "No clue",
        "Don't know what that is, but I don't like it",
        "Not in the database. Thankfully.",
        "Can't find it",
        "It's a big nope"
    };

    public static string DefinitionToText(ResourceDefinition definition)
    {
        if (definition.ObjectType == ObjectType.am6thing)
            return IDontKnow.PickRandom();

        var baseDesc = "It's a ";
        var hasNoise = false;

        if (definition.Shape != Shape.na)
            baseDesc += $"{EnumAdapter(definition.Shape.ToString())} ";
        else if (StaticHelpers.Flip())
        {
            baseDesc += $"{NoiseCategoriesAdj.PickRandom()} ";
            hasNoise = true;
        }

        baseDesc += $"{EnumAdapter(definition.ObjectType.ToString())}";

        if (!hasNoise && StaticHelpers.Flip())
        {
            baseDesc += $"{NoiseCategoriesAppend.PickRandom()}";
            hasNoise = true;
        }

        if (!baseDesc.EndsWith("?"))
            baseDesc += $".";

        if (definition.ObjectType == ObjectType.am6cabinet)
        {
            baseDesc += " It's got stuff inside.";
        }

        return baseDesc;
    }
    public static string GetArrestMessage(bool correct, string name)
    {
        return correct? $"YOU GOT {name.ToUpper()}, THE THEIF!" +
            $"{Environment.NewLine}Alas, the stolen cookie was just a DECOY." +
            $"{Environment.NewLine}The real one might be in the next town..."
            : $"OUCH, THAT WAS NOT THE THEIF. {name} was innocent...";
    }

    // Menus
    // TODO move all the menu texts here
    public static string ArrestTimeout = "Oh no... Time's up. The theif ate the cookie.";


    // Manual lines
    public static string ManualShape(string adjective, string objectType, bool liars) =>
        $"Has {EnumAdapter(adjective)} {EnumAdapter(objectType)} = {(liars ? "lies" : "NOT lies")}";

    public static string ManualObjectType(string objectType, bool liars) =>
        $"Has {EnumAdapter(objectType)} = {(liars ? "lies" : "NOT lies")}";

    //public static string ManualHasOrNot(bool have, string objectType, bool liars) =>
    //    $"Those who {(have? "have" : "do not have")} a {EnumAdapter(objectType)} are {(liars ? "liars" : "not liars")}";

    private static string EnumAdapter(string target)
    {
        return target.Replace("_", " ").Replace("am6", "");
    }

    public static string GetDialogue(CharacterInfo character, DialogueType characterDialogueType, CharacterInfo otherCharacter = null)
    {
        if (characterDialogueType == DialogueType.TalkOthers && otherCharacter != null)
        {
            // Will claim the other is innocent if the other is the thief and they are liars, or if they are not liars and the other is not a thief
            var claimsInnocence = character.Liar == otherCharacter.IsThief;
            return claimsInnocence ?
                LineAboutOthersDidNotSteal(otherCharacter.Name) : LineAboutOthersStole(otherCharacter.Name);

        }
        else if (characterDialogueType == DialogueType.TalkSelf)
        {
            // Will claim to be innocent if they are either a thief and a liar, or if they are not the thief and not a liar
            var claimsInnocence = character.IsThief == character.Liar;

            return claimsInnocence ?
                LineAboutSelfDidNotSteal() : LineAboutSelfStole();
        }

        throw new InvalidOperationException($"Unknown {characterDialogueType}");
    }
}
