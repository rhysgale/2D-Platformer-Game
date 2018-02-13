internal enum TextureType
{
    DebugLine,
    DebugCircle,

    //user interface
    TopBar,
    Background,

    //MapBits
    Floor,

    //Character
    IdleRight,
    IdleLeft,
    RunRight,
    RunLeft,

    //MapObjects
    RedGem,
    BlueGem,

    //Obstacles
    MovingSaw,
    NotDrawn,
    MainMenuBackground,
    CavemanRunRight,
    CavemanRunLeft,
    CastleDoor,
    Lever,
    Projectile,
    GreenBar,
    RedBar,
    Crate
}

internal enum Direction
{
    Up, 
    Down, 
    Left,
    Right
}

internal enum FontType
{
    Fifty,
    Fourty,
    Thirty,
    TwentyFive,
    Twenty,
    Fifteen,
    Ten
}

internal enum Selection
{
    PlayGame,
    Settings,
    Exit
}

internal enum GameMode
{
    Paused,
    InGame,
    MainMenu,
    Settings
}

internal enum Settings
{
    ShowPaths,
    ShowBoundingBoxes,
    Back
}
