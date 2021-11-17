using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPIIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebSocketsController : ControllerBase
    {
        private readonly ILogger<WebSocketsController> _logger;
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();
        public class Game
        {
            public Game()
            {
                Games = new List<dynamic>();
                GameId = 0;
                action = "onRequestGames";
                messageType = "onServerMsg";
            }

            public List<dynamic> Games { get; set; }
            public int GameId { get; set; }
            public string action { get; set; }
            public string messageType { get; set; }
        }
        private static Game games = new Game();


        public WebSocketsController(ILogger<WebSocketsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                string socketId;
                do
                {
                    socketId = Guid.NewGuid().ToString();

                } while (!_sockets.TryAdd(socketId, webSocket));


                await Echo(socketId, webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task Echo(string socketId, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                string msg = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                System.Diagnostics.Debug.WriteLine(msg);
                try
                {
                    JObject data = JObject.Parse(msg);
                    string jsonStr;
                    byte[] serverMsg;
                    switch ((string)data["messageType"])
                    {
                        case "getId":
                            data["action"] = "onGetId";
                            data["messageType"] = "onServerMsg";
                            data["id"] = socketId;
                            //await sendMessage(data, webSocket);
                            jsonStr = JsonConvert.SerializeObject(data).ToString();
                            System.Diagnostics.Debug.WriteLine(jsonStr);
                            serverMsg = Encoding.UTF8.GetBytes(jsonStr);
                            await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                            break;
                        case "sendToClients":
                            data["messageType"] = "onClientMsg";
                            jsonStr = JsonConvert.SerializeObject(data).ToString();
                            System.Diagnostics.Debug.WriteLine(jsonStr);
                            serverMsg = Encoding.UTF8.GetBytes(jsonStr);
                            WebSocket ws;

                            foreach (string client in data["playerIds"])
                            {
                                if (client != socketId)
                                {
                                    if (_sockets.TryGetValue(client, out ws))
                                    {
                                        await ws.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                                            WebSocketMessageType.Text, true, CancellationToken.None);
                                    }
                                }
                            }
                            break;
                        case "sendToServer":
                            data["messageType"] = "onServerMsg";
                            if ((string)data["action"] == "getId")
                            {
                                data["action"] = "onGetId";
                                data["id"] = socketId;
                                jsonStr = JsonConvert.SerializeObject(data).ToString();
                                System.Diagnostics.Debug.WriteLine(jsonStr);
                                serverMsg = Encoding.UTF8.GetBytes(jsonStr);
                                await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                break;
                            }
                            if ((int)data["nrPlayers"] == 1)
                            {
                                // Sologame send start and do nothing else
                                data["playerIds"][0] = socketId;
                                data["action"] = "onGameStart";
                                data["gameId"] = games.GameId++;
                                //await sendMessage(data, webSocket);
                                jsonStr = JsonConvert.SerializeObject(data).ToString();
                                System.Diagnostics.Debug.WriteLine(jsonStr);
                                serverMsg = Encoding.UTF8.GetBytes(jsonStr);
                                await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                                break;
                            }
                            var foundGame = -1;
                            for (var i = 0; i < games.Games.Count; i++)
                            {
                                if (
                                  data["gameType"] == games.Games[i]["gameType"] &&
                                  data["nrPlayers"] == games.Games[i]["nrPlayers"] &&
                                  (int)games.Games[i]["connected"] < (int)games.Games[i]["nrPlayers"]
                                )
                                {
                                    foundGame = i;
                                    break;
                                }
                            }
                            if (foundGame == -1)
                            {
                                data["playerIds"][0] = socketId;
                                data["connected"] = 1;
                                data["gameId"] = games.GameId++;
                                games.Games.Add(data);
                            }
                            else
                            {
                                // If id already present do nothing
                                //System.Diagnostics.Debug.WriteLine(games.Games[foundGame]);
                                var tmp = games.Games[foundGame]["playerIds"].ToObject<List<string>>();
                                if (tmp.Contains(socketId))
                                {
                                    break;
                                }
                                games.Games[foundGame]["playerIds"][(int)games.Games[foundGame]["connected"]] = socketId;
                                //games[foundGame]["playerIds"].push(data["playerIds"][0]);
                                games.Games[foundGame]["connected"] = (int)games.Games[foundGame]["connected"] + 1;

                                if (games.Games[foundGame]["nrPlayers"] == games.Games[foundGame]["connected"])
                                {
                                    var game = games.Games[foundGame];
                                    //games.splice(i, 1);
                                    // Send start game to players
                                    game["action"] = "onGameStart";

                                    for (var i = 0; i < game["playerIds"].ToObject<List<string>>().Count; i++)
                                    {
                                        if (_sockets.TryGetValue((string)game["playerIds"][i], out ws))
                                        {
                                            jsonStr = JsonConvert.SerializeObject(game).ToString();
                                            System.Diagnostics.Debug.WriteLine(jsonStr);
                                            serverMsg = Encoding.UTF8.GetBytes(jsonStr);
                                            await ws.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                                            //await sendMessage(game, ws);
                                        }
                                    }
                                }
                            }

                            //string jsonString = @"{games: games}";

                            //data = new JObject();
                            //data["games"] = JsonConvert.SerializeObject(games);
                            //data["action"] = "onRequestGames";
                            //await sendMessage(data, webSocket);

                            jsonStr = JsonConvert.SerializeObject(games).ToString();
                            System.Diagnostics.Debug.WriteLine(jsonStr);
                            serverMsg = Encoding.UTF8.GetBytes(jsonStr);
                            await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                            break;
                    }
                }
                catch (Exception e)
                {
                    JObject json = new JObject();
                    json["error"] = e.ToString();
                    //json["games"] = JsonConvert.SerializeObject(games);
                    json["message"] = msg;
                    string jsonStr = JsonConvert.SerializeObject(json).ToString();
                    System.Diagnostics.Debug.WriteLine(jsonStr);
                    var serverMsg = Encoding.UTF8.GetBytes(jsonStr);
                    await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    //sendMessage(json, webSocket);
                }



                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                System.Diagnostics.Debug.WriteLine(result);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        //private async Task sendMessage(JObject data, WebSocket webSocket)
        //{
        //    string jsonStr = JsonConvert.SerializeObject(data).ToString();
        //    System.Diagnostics.Debug.WriteLine(jsonStr);
        //    var serverMsg = Encoding.UTF8.GetBytes(jsonStr);
        //    await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        //}
    }
}
