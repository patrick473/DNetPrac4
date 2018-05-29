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
            var origTitles = CDXml.Descendants("track").SelectMany(x => x.Descendants("title"));
            List<String> origTitleValues = new List<string>();
            Console.WriteLine("-----------------------------Titles not in previous album----------------------------");

            foreach (var element in origTitles) {
                origTitleValues.Add(element.Value);
            }

            String xmlString;
            using (WebClient wc = new WebClient()) {
                xmlString = wc.DownloadString(@"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=b5cbf8dcef4c6acfc5698f8709841949&artist=BTS&album=You+Never+Walk+Alone");
            }
            XDocument myXMLDoc = XDocument.Parse(xmlString);
            //get tracks
            List<XElement> tracksXML = myXMLDoc.Descendants("track").ToList();

            var query =

                from tr in myXMLDoc.Descendants("track")

                where
                !((from tr2 in CDXml.Descendants("track")
                  where tr.Element("name").Value == tr2.Element("title").Value
                    
                  select tr2).Any())

                
                select tr.Element("name").Value
                ;
            foreach (String tp in query) {
                /*Track tf = new Track {
                    title = tr.Element("name").Value,

                    artist = tr.Element("artist").Element("name").Value,
                    length = TimeSpan.FromSeconds(Int32.Parse(tr.Element("duration").Value))
                };*/
                Console.WriteLine(tp);
            }
            /*
            foreach (XElement el in tracksXML) {
                string name = el.Element("name").Value;
                //needed to parse to int timespan not parsable from string
                TimeSpan duration = TimeSpan.FromSeconds(Int32.Parse(el.Element("duration").Value));
                string artist = el.Element("artist").Element("name").Value;
                //compare tracks
                if (!origTitleValues.Contains(name)) {
                   // Console.WriteLine(el.Element("name").Value);
                    Track track = new Track {
                        artist = artist,
                        length = duration,
                        title = name
                    };
                    cd1.tracks.Add(track);
                }
            }
            */
            Console.WriteLine("-----------------------------------FILLED ALBUM----------------------------------");
            //Console.WriteLine(cd1.generateXML().ToString());
            Console.Read();
        }

    }
}
