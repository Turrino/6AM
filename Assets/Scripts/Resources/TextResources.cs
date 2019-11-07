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

    //TODO could add a bit of variation here
    public static string LineAboutSelfStole => $"I stoled the cookie ;)";
    public static string LineAboutSelfDidNotSteal => $"I did not stole the cookie!!1";
    public static string LineAboutOthersStole(string otherCharacter) => $"{otherCharacter} is teh thief.";
    public static string LineAboutOthersDidNotSteal(string otherCharacter) => $"{otherCharacter} is innosent.";

    //public static string EnrichDialogue(LocationInfo locInfo, string originalDialogue)
    //{

    //}

    //private static string Enrich1 = "Hey do you like my place?";
    //private static string Enrich1 = "How do you do?";
    //private static string Enrich1 = "Hey bud.";
    //private static string Enrich1 = "Do you like my REPL";
    //private static string Enrich1 = "";

    // this just makes the text a bit silly and gives no information
    public static string[] NoiseCategoriesAdj =
        { "high-end", "beautiful", "hypnotic", "dazzling", "DIY", "horrible",
        "talking", "serious", "glorious", "so-so", "vegetarian", "stray", "smelly",
        "second-hand", "bullet-proof", "high-tech", "primitive" };
    public static string[] NoiseCategoriesAppend =
        { " made by an elephant", ", as seen on TV", ", hope you like it", ", looks legit to me",
        ". Really ties the room together", ", it has wifi", ". Can I go home now?",
        ". Are we done yet?", ". Do you like it?", ", always wanted one like that",
        ", like all the other ones", " in all its glory", ", like it or not"};

    public static string DefinitionToText(ResourceDefinition definition)
    {
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
        return correct? $"YOU GOT THE THEIF! {name} stole the cookies."
            : $"WHOOPS, THAT WAS NOT THE THEIF. {name} was innocent...";
    }

    // Menus
    // TODO move all the menu texts here
    public static string ArrestTimeout = "Oh snap... Time's up. The theif ate the cookie. GG mate.";


    // Manual lines
    public static string ManualShape(string adjective, string objectType, bool liars) =>
        $"Those who have a {EnumAdapter(adjective)} {EnumAdapter(objectType)} are {(liars ? "liars" : "not liars")}";

    public static string ManualObjectType(string objectType, bool liars) =>
        $"Those who have a {EnumAdapter(objectType)} are {(liars ? "liars" : "not liars")}";

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
                LineAboutSelfDidNotSteal : LineAboutSelfStole;
        }

        throw new InvalidOperationException($"Unknown {characterDialogueType}");
    }
}
