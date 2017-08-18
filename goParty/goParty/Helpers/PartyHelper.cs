using goParty.Models;
using Microsoft.Azure.Documents.Spatial;
using System;
using System.Collections.Generic;
using System.Spatial;
using System.Text;

namespace goParty.Helpers
{
    class PartyHelper
    {
        public static List<PartyDetailsDB> DummyPartyDetails()
        {
            List<PartyDetailsDB> tempDummyList = new List<PartyDetailsDB>();

            tempDummyList.Add(new PartyDetailsDB
            {
                userId = 1.ToString(),
                partyId = Guid.NewGuid().ToString(),
                title = "Preps & Crooks",
                picture = "p1.jpg",
                description = "Join our Wicked party  Saturday night. Doors open at 22:00. 'Crooks' refers to a more 90's thuggish style.",
                type = 1,
                price = 50,
                latt = 55.683322,
                lon = 12.564556,
                when = DateTime.Parse("2017-07-29 22:00:00"),
                where = "Nansensgade; 41; 1366; København K;",
                rating = 0,
                ageMax = 0,
                ageMin = 0,
                location = new Point(55.683322, 12.564556)
                //, 22, 100, 0, 50
            });

            tempDummyList.Add(new PartyDetailsDB
            {
                userId = 1.ToString(),
                partyId = Guid.NewGuid().ToString(),
                title = "Dakkedak",
                picture = "p2.jpg",
                description = "The best parties are the ones you can't remember, with the people you'll never forget!",
                type = 1,
                price = 0,
                latt = 55.69149,
                lon = 12.57453,
                when = DateTime.Parse("2017-07-29 23:00:00"),
                where = "Abildgaardsgade; 26; 2100; København Ø;",
                rating = 0,
                ageMax = 30,
                ageMin = 0,
                location = new Point(55.69149, 12.57453)

                //, 22, 100, 0, 50
            });

            tempDummyList.Add(new PartyDetailsDB(3, 1, "AfroNight", 3, "Mamajuana Edibles Presents 'The Good Life' Reggae Smokefest Saturday July 29th from 12p to 4p! Come enjoy the wonderful sounds of carribean music and take flight with your friends. We will have delicious edibles, flowers, music, raffles, pre-rolls, great ", 1, '0', 55.67991, 12.602083, "2017-07-29 12:00:00", "Masteskursvej; 1434; København K\n", 0, 300, 0, 0, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(4, 2, "CollegeBrawl", 4, "Hundreds of people. Dozens of kegs. Every door you open leads to an orgy. Always black out. Never make it home. Can always get on the beer pong table.", 1, 100, 55.662895, 12.534849, "2017-07-29 18:00:00", "Rektorparken; 1; 2450; København SV;\n", -35, 350, 3, 100, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(5, 1, "APA feat. Dj MyStiquE", 5, "B&W Hallerne presents APA feat. Dj MyStiquE. Tickets through the app", 0, 200, 55.692275, 12.616634, "2017-07-29 19:00:00", "Refshalevej; 177A; 1432; København K;", 18, 500, 0, 200, "2017-07-29 23:00:00"));
            tempDummyList.Add(new PartyDetailsDB(6, 2, "Radisson presents: Axeltorv Party", 6, "Axeltorv introduces: Axeltorv Party with an open stage and music all night long.", 3, 50, 55.675835, 12.564571, "2017-07-29 19:00:00", "Vesterbrogade; 2; 1550; København V;", 18, 600, 0, 50, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(7, 1, "Bikers party", 7, "Biker Summers Party. We have the girls!", 1, 0, 55.649979, 12.62093, "2017-07-29 16:00:00", "Kastrupvej; 118; 2300; København S;", 30, 120, 0, 0, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(8, 2, "ConfettiBlast", 8, "Join the Confetti themed party with along with your friends. Inspired by extravangance and fun. This night will not be easily forgotten", 0, 80, 55.672966, 12.576, "2017-07-29 20:00:00", "Vester Voldgade; 96; 1552; København V;", 20, 0, 0, 80, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(9, 1, "Party At The Happy Pig", 9, "Continue the party at The Happy Pig. We know how to through a festive time and invite you to a night with fun and booze.", 0, 0, 55.680116, 12.574593, "2017-07-29 18:00:00", "Lille Kannikestræde; 3; 1170; København K;", 0, 360, 0, 0, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(10, 2, "Astro music", 10, "Two soundsystems  with over 10 hours of Dj'ing covers a broad range of genres, including techno and acid techno, hard techno, industrial hardcore, hard d'n'b, breakcore, speedcore and more. It is going to be wicked!!", 0, 60, 55.681007, 12.576396, "2017-07-29 19:00:00", "Skindergade; 1; 1159; København K;", 20, 0, 0, 60, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(11, 1, "K-pop Party", 11, "Buska, Dtm, Neoco, Matim and more are playing K-pop at our venue tonight. Be sure to stop by. Entrance is 30 DKKR", 0, 30, 55.682602, 12.578921, "2017-07-29 16:00:00", "Vognmagergade; 8; 1120; København K;", 0, 260, 0, 30, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(12, 2, "White Sensation at Vega", 12, "Sensation at Vega include the artists: Tiësto, Armin van Buuren, Swedish House Mafia, Hardwell this night. Get your ticket now, limited room left.", 0, 400, 55.667834, 12.544211, "2017-07-29 19:00:00", "Enghavevej; 40; 1674; København V;", 0, 600, 0, 400, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(13, 1, "Lille Vega presents Maker", 13, "Maker is a Norwegian folk group with an amazing sing and song background. Tonight they are performing at Lille Vega, and we are excited to announce the party through this app.", 0, 250, 55.667834, 12.544211, "2017-07-29 16:00:00", "Enghavevej; 40; 1674; København V;", 0, 230, 0, 250, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(14, 2, "Hat Parade", 14, "A silly hat themed home party with great snacks. Everybody is welcome.", 1, 0, 55.675669, 12.567169, "2017-07-29 18:00:00", "Vesterbrogade; 2A; 1620; København V;", 0, 50, 0, 0, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(15, 1, "A-Bar", 15, "A-Bar is once again opening the doors for one hell of a party. Why don't you come downunder?", 0, 40, 55.677907, 12.570664, "2017-07-29 20:00:00", "Vestergade; 10; 1456; København K;", 16, 350, 0, 40, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(16, 2, "Live music at Drunken Flamingo", 16, "Live music at Drunken Flamingo and a free drink once you are in! We hope to see you. Music tonight is Summerthemed.", 0, 40, 55.678323, 12.571858, "2017-07-29 18:00:00", "Gammeltorv; 14; 1457; København K;", 0, 0, 0, 0, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(17, 1, "Fun at home", 17, "Drinks are on me… Until I run out :) Come by, we are always happy to meet new people.", 1, 50, 55.678326, 12.575458, "2017-07-29 19:00:00", "Vimmelskaftet; 43B; 1161; København K;", 0, 30, 0, 50, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(18, 2, "House Warming", 18, "I'm am trying this new app as an invite to my house warming tonight. We might get to meet some other people than our usual crew. See you later!", 1, 0, 55.678703, 12.573647, "2017-07-29 19:00:00", "Skoubogade; 4; 1158; KØBENHAVN K;", 0, 0, 4, 0, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(19, 1, "Stamp party at Voodoo Lounge", 19, "Voodoo Lounge is launching an offer never seen before. Arrive before 17:00 and drink for free all night! Arrive later and get 2 drinks for free. Happy partying. #YOLO. (Drinks available at extra cost).", 0, 100, 55.679378, 12.576295, "2017-07-29 17:00:00", "Valkendorfsgade; 22; 1151; København K;", 0, 250, 0, 100, "0000-00-00 00:00:00"));
            tempDummyList.Add(new PartyDetailsDB(20, 2, "Wild Party!", 20, "Come to my wild party! Its a road Party! The block and the hood are joining and there will be plenty of music to set the mood! Welcome", 4, 0, 55.680104, 12.575944, "2017-07-29 19:00:00", "Gråbrødretorv; 8; 1154; København K;", 0, 0, 0, 0, "0000-00-00 00:00:00"));

            return tempDummyList;
        }

    }
}
