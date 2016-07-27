/* lcmp 1.0 [literally (a) console music player]
          licensed under GNU GPL 3.0          */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Threading;
using NAudio.Wave;

namespace lcmp
{
    class Program
    {
        public string[] playlist;
        public MP3Playback mp3 = new MP3Playback();
        public int curleft, curtop, curindex;
        enum Response { LOADED_PLAYLIST, QUIT, CANNOT_PARSE, LOADING_FAILED, DEFAULT }

        //Callback for the SongCheck timer present in PlaylistOperate
        private void TimerCallback(Object o)
        {
            if (mp3.waveOutDevice.PlaybackState == PlaybackState.Stopped)
            {
                mp3.Stop();
                mp3.Dispose();
                curindex++;
                playlistOperate();
            }

            GC.Collect();
        }

        private Response playlistLoad()
        {
            Console.Clear();
            Utils.writeCenteredColLine("Path to playlist", ConsoleColor.Red);
            Utils.writeColored(":", ConsoleColor.White);
            string tmppath = Console.ReadLine();

            try
            {
                object files = Directory.GetFiles(tmppath, "*.*", SearchOption.AllDirectories)
                                                .Where(s => s.EndsWith(".mp3")); //ogg implementation will be added

                playlist = ((IEnumerable)files).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();

                curindex = 0;
                playlistOperate();
                return Response.LOADED_PLAYLIST;
            }
            catch
            {
                playlistLoad();
                return Response.LOADING_FAILED;
            }
        }

        private Response Parse(string command, int priviledgeLevel)
        {
            switch (command)
            {
                case "q":
                    Environment.Exit(0);
                    return Response.QUIT;

                case "o":
                    return playlistLoad();
            
                case "n":
                    if (priviledgeLevel == 2)
                    {
                        mp3.Stop();
                        mp3.Dispose();
                        curindex++;
                        playlistOperate();
                    }else{}
                    return Response.DEFAULT;

                case "p":
                    if (priviledgeLevel == 2)
                    {
                        mp3.Stop();
                        mp3.Dispose();
                        curindex--;
                        playlistOperate();
                    }else{}
                    return Response.DEFAULT;

                case "l":
                    if (priviledgeLevel == 2)
                    {
                        mp3.Stop();
                        mp3.Dispose();

                        Console.Clear();
                        Utils.writeCenteredColLine("Load index", ConsoleColor.Cyan);
                        Utils.writeColored(":", ConsoleColor.White);

                        curindex = Convert.ToInt32(Console.ReadLine());
                        playlistOperate();

                    }else { }
                    return Response.DEFAULT;
                case "s":
                    if (priviledgeLevel == 2)
                    {
                        mp3.Stop();
                        mp3.Dispose();
                        Console.Clear();
                        Utils.writeColoredLine("Search:", ConsoleColor.Yellow);
                        Utils.writeColored(":", ConsoleColor.White);
                        string srch = Console.ReadLine();
                        object objrls = Array.FindAll(playlist, s => s.IndexOf(srch, StringComparison.OrdinalIgnoreCase) != -1);
                        string[] results = ((IEnumerable)objrls).Cast<object>()
                                                                 .Select(s => s.ToString())
                                                                 .ToArray();
                        int i = 0;
                        Utils.writeColoredLine("Results:\n", ConsoleColor.Yellow);
                        foreach (string line in results)
                        {
                            try
                            {
                                Utils.writeColoredLine(i + ": " + results[i], ConsoleColor.White);
                            }
                            catch { }
                            i++;
                        }
                        Utils.writeColored(":", ConsoleColor.White);
                        int pick = Convert.ToInt32(Console.ReadLine());
                        curindex = Array.IndexOf(playlist, results[pick]);
                        playlistOperate();
                    }
                    else { }
                    return Response.DEFAULT;

                default:
                    return Response.CANNOT_PARSE;
            }
        }
        private void playlistOperate()
        {
            Console.Clear();
            Console.Title = "lcmp 1.0 - playing";
            Utils.writeCenteredColLine("lcmp 1.0", ConsoleColor.Blue);
            Utils.writeCenteredColLine("Currently playing: " + playlist[curindex], ConsoleColor.White);
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorLeft + 5);
            
            Utils.writeColoredLine("--------------------------------------------------------------------------------", ConsoleColor.Green);
            Utils.writeColoredLine("Next: ", ConsoleColor.Red);
            
            try { Utils.writeColoredLine(playlist[curindex + 1], ConsoleColor.Yellow); //yeah, for loop wasn't working
            Utils.writeColoredLine(playlist[curindex + 2], ConsoleColor.Yellow);
            Utils.writeColoredLine(playlist[curindex + 3], ConsoleColor.Yellow);
            Utils.writeColoredLine(playlist[curindex + 4], ConsoleColor.Yellow);
            Utils.writeColoredLine(playlist[curindex + 5], ConsoleColor.Yellow);
            Utils.writeColoredLine(playlist[curindex + 6], ConsoleColor.Yellow);
            Utils.writeColoredLine(playlist[curindex + 7], ConsoleColor.Yellow); 
            }
            catch { }
            
            mp3.Load(playlist[curindex]);
            mp3.Play();
            Timer songCheck = new Timer(TimerCallback, null, 0, 1000);

            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
            Utils.writeColored(":", ConsoleColor.White);
            string command = Console.ReadLine();
            songCheck.Dispose();
            Parse(command,2);
        }


        /* Main void. Does all the things. Trust me.
                        lcmp 1.0                  */
        static void Main(string[] args)
        {
            Console.Title = "lcmp 1.0";
            Console.Clear();
            Console.SetBufferSize(80, 25);
            Console.SetWindowSize(80, 25);
            Program lcmp = new Program(); //we need this because static
            lcmp.curleft = Console.CursorLeft;
            lcmp.curtop = Console.CursorTop;

            if (args == null || args.Length == 0)
            {
                Utils.writeCenteredColLine("lcmp 1.0", ConsoleColor.Yellow);
                Utils.writeCenteredColLine("Nothing playing", ConsoleColor.White);

                Console.CursorTop = Console.WindowTop + Console.WindowHeight - 1;
                Console.Write(":");

                string cmd = Console.ReadLine();
                Response rsp = lcmp.Parse(cmd, 1);
                if (rsp == Response.CANNOT_PARSE || rsp == Response.DEFAULT)
                {
                    GC.Collect();
                    Main(args);
                }
            }
            else
            {
                object files = Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".mp3") || s.EndsWith(".ogg"));
                lcmp.playlist = ((IEnumerable)files).Cast<object>()
                             .Select(x => x.ToString())
                             .ToArray();
                lcmp.playlistOperate();
            }
        }
    }
}
