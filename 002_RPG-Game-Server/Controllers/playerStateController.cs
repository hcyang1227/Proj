using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _002_RPG_Game_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class playerStateController : ControllerBase
    {
        private readonly ILogger<playerStateController> _logger;

        public playerStateController(ILogger<playerStateController> logger)
        {
            _logger = logger;
        }

        public class Vector3
        {
            public float x;
            public float y;
            public float z;
            public Vector3(float x, float y, float z)
            {}
        }

        public class playerState
        {
            public float m_HP;
            public int m_Money;
            public int m_Potion;
            public int m_facingDirection;
            public Vector3 m_position;
            public int gameHistory;
            public bool SrchCrate;
            public Vector3[] m_platform;
            public Vector3[] m_enemy;
            public float[] m_HPs;
        }

        [HttpGet]
        public playerState Get()
        {
            playerState myPlayer = new playerState();
            myPlayer.m_HP = 100.0f;
            myPlayer.m_Money = 300;
            myPlayer.m_Potion = 0;
            myPlayer.m_facingDirection = 1;
            myPlayer.m_position = new Vector3(148.65f, 62.09f, 8.01f);
            myPlayer.gameHistory = 0;
            myPlayer.SrchCrate = true;
            return myPlayer;
        }
    }
}
