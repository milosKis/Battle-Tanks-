using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    static class Constants
    {
        //PlayerController
        public const float MaxCoordinate = 39;
        public const float MinCoordinate = 1;
        public const string RocketCompPrefabDest = "Rocket_Red";
        public const string RocketPlayerPrefabDest = "Rocket_Blue";
        public const string HorizontalAxis = "Horizontal";
        public const string VerticalAxis = "Vertical";

        //DestroyOutOfBounds
        public const string ExplosionParticleDest = "Particles\\Explosion01";
        public const string ExplosionParticleSmokeDest = "Particles\\RocketGoneSmoke";

        //MapManager
        public const int ObjectTypeIndex = 0;
        public const int NumOfRows = 20;
        public const int NumOfColums = 20;
        public const int MaxDistance = 1000;
        public const int MapRowIndex = 1;
        public const int MapColumnIndex = 2;
        public const string SourcePrefix = "Assets\\Maps\\Map";
        public const string FileExtension = ".txt";
        public const char SplitSign = ' ';

        //GameManager
        public const string Canvas = "Canvas";
        public const string TextGameOver = "GameOverTitle";
        public const string Background = "Background";
        public const string TextTitle = "TextTitle";
        public const string Menu = "Menu";
        public const string MainMenu = "MainMenu";
        public const string ButtonLevel = "ButtonLevel";
        public const string ButtonSimulation = "ButtonSimulation";
        public const string InfoFilePath = "Assets\\Highest Level Info\\Highest Level Info.txt";

        //CreationManager
        public const string BrickBlockDest = "BrickBlock";
        public const string BoxDest = "Box";
        public const string GrassBlockDest = "GrassBlock";
        public const string WaterBlockDest = "WaterBlock";
        public const string BlueCastleDest = "ForestCastle_Blue";
        public const string RedCastleDest = "ForestCastle_Red";
        public const string PlayerTankDest = "Tanks\\PlayerTank";
        public const string BasicScoutTankDest = "Tanks\\BasicScoutTank";
        public const string BaseTrackerTankDest = "Tanks\\BaseTrackerTank";
        public const string PlayerTrackerTankDest = "Tanks\\PlayerTrackerTank";

        //CollisionManager
        public const float CollisionOffset = 2;
        public const float MaxDifferenceForCoordinates = 0.1f;
        public const float MaxDifferenceForAngles = 1.0f;
        public const float AngleUp = 270;
        public const float AngleDown = 90;
        public const float AngleLeft = 180;
        public const float AngleRight = 360;
        public const string FailedExplosionParticleDest = "Particles\\FailedExplosion";
        public const string BoxTag = "Box";
        public const string ComputerTag = "ComputerTank";
        public const string PlayerTag = "Player";
        public const string RocketCompTag = "RocketRed";
        public const string RocketPlayerTag = "RocketBlue";
        public const string BrickBlockTag = "BrickBlock";
        public const string WaterBlockTag = "WaterBlock";
        public const string GrassBlockTag = "GrassBlock";
        public const string BlueCastleTag = "ForestCastle_Blue";
        public const string BasicScoutTag = "BasicScout";
        public const string BaseTrackerTag = "BaseTracker";
        public const string PlayerTrackerTag = "PlayerTracker";

        //ComputerTank
        public const float TurnAroundAngle = 180;
        public const float MoveLength = 0.125f;
        public const float UpdateMethodDelay = 0.05f;
        public const int CastleMinRow = 17;
        public const int CastleMinColumn = 8;
        public static int[] CastleRows = {17, 17, 18, 18};
        public static int[] CastleColumns = {9, 10, 8, 11};
    }
}
