using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace DNetPrac4
{
    internal class Program {
        private static void Main(string[] args) {
            Track track1 = new Track {
                artist = "BTS",
                length = TimeSpan.FromSeconds(121),
                title = "Intro : Boy Meets Evil"
            };
            Track track2 = new Track {
                artist = "BTS",
                length = TimeSpan.FromSeconds(216),
                title = "Blood Sweat & Tears"
            };
            Track track3 = new Track {
                artist = "BTS",
                length = TimeSpan.FromSeconds(229),
                title = "Begin"
            };
            CD cd1 = new CD {
                title = "You Never Walk Alone",
                artist = "BTS",
                tracks = new List<Track> { track1, track2, track3 }
            };

            XDocument CDXml = cd1.generateXML();
            Console.WriteLine(CDXml.ToString());

            // PART 2 FROM HERE ON

            // Retrieve original titles for comparing
            
            Console.WriteLine("-----------------------------Titles not in previous album----------------------------");

            String xmlString;
            using (WebClient wc = new WebClient()) {
                xmlString = wc.DownloadString(@"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=b5cbf8dcef4c6acfc5698f8709841949&artist=BTS&album=You+Never+Walk+Alone");
            }
            XDocument myXMLDoc = XDocument.Parse(xmlString);
            //get tracks
           

            var query =

                from tr in myXMLDoc.Descendants("track")

                where
                !((from tr2 in CDXml.Descendants("track")
                  where tr.Element("name").Value == tr2.Element("title").Value &&
                  tr.Element("artist").Element("name").Value == tr2.Element("artist").Value
                    
                  select tr2).Any())

                
                select new Track {
                    title = tr.Element("name").Value,

                    artist = tr.Element("artist").Element("name").Value,
                    length = TimeSpan.FromSeconds(Int32.Parse(tr.Element("duration").Value))
                };
           foreach(Track tf in query){
                cd1.tracks.Add(tf);
                Console.WriteLine(tf.title);

            }
            
            Console.WriteLine("-----------------------------------FILLED ALBUM----------------------------------");
            Console.WriteLine(cd1.generateXML().ToString());
            Console.Read();
        }

    }
}
