using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Gum.Forms;
using Gum.Forms.Controls;
using MonoGameGum;
namespace the_road_of_death;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D personaje;
    private Vector2 posicion;

     private Texture2D personaje2;
    private Vector2 posicion2;
    private const float alturaPersonaje = 70f; // altura final en pantalla (en píxeles)
    private const float suelo = 400f;
    private float escala1;
    private float escala2;
    private Rectangle limites1;
    private Rectangle limites2;
    private Texture2D fondo;
    private Texture2D _fondoInterfaz;
    private Texture2D pixel;
    private Rectangle[] botones;
    private int totalFrames = 8;
    private int frameAncho, frameAlto;
    private int frameActual1 = 0;
    private double temporizador1 = 0;
    private double tiempoPorFrame = 0.1; // segundos por cuadro
    private bool caminando1 = false;
    private SpriteEffects efecto1 = SpriteEffects.None;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        posicion = new Vector2(100, 300);
        posicion2 = new Vector2(400, 300);
        base.Initialize();
       
        base.Initialize();

        GumService.Default.Initialize(this, DefaultVisualsVersion.V3);
         CrearMenu();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    
        string ruta = Path.Combine(
        AppContext.BaseDirectory,
        "content",
        "correr.png"
        );
    
        using (FileStream archivo = File.OpenRead(ruta))
        {
            personaje = Texture2D.FromStream(GraphicsDevice, archivo);
        }
        frameAncho = personaje.Width / totalFrames;
        frameAlto = personaje.Height;
        string ruta2 = Path.Combine(
        AppContext.BaseDirectory,
        "content",
        "personaje2.png"
         );

        using (FileStream archivo2 = File.OpenRead(ruta2))
      {
        personaje2 = Texture2D.FromStream(GraphicsDevice, archivo2);
      }
         
         string rutaFondo = Path.Combine(
         AppContext.BaseDirectory,
        "content",
         "fondo.png"
        );

        using (FileStream archivoFondo = File.OpenRead(rutaFondo))
        {
         fondo = Texture2D.FromStream(GraphicsDevice, archivoFondo);
        }
          
         
         limites1 = ObtenerLimitesVisibles(personaje);
         limites2 = ObtenerLimitesVisibles(personaje2);
        
         escala1 = alturaPersonaje / limites1.Height;
         escala2 = alturaPersonaje / limites2.Height;

         posicion.Y = suelo - (limites1.Y + limites1.Height) * escala1;
         posicion2.Y = suelo - (limites2.Y + limites2.Height) * escala2;
 
        string rutaFondoInterfaz = Path.Combine(AppContext.BaseDirectory, "content", "fondointerfaz.png");
        using (FileStream archivoFondoInterfaz = File.OpenRead(rutaFondoInterfaz))
         {
         _fondoInterfaz = Texture2D.FromStream(GraphicsDevice, archivoFondoInterfaz);
         }

           // Menú: pixel y botones
          pixel = new Texture2D(GraphicsDevice, 1, 1);
         pixel.SetData(new[] { Color.White });

         int ancho = 300;
         int alto = 60;
         int x = GraphicsDevice.Viewport.Width / 2 - ancho / 2;
         int centroY = GraphicsDevice.Viewport.Height / 2;

          botones = new Rectangle[]
          {
        new Rectangle(x, centroY - 40, ancho, alto),  // Jugar
        new Rectangle(x, centroY + 40, ancho, alto)   // Salir
         };
    }

    private Rectangle ObtenerLimitesVisibles(Texture2D textura)
{
    Color[] pixeles = new Color[textura.Width * textura.Height];
    textura.GetData(pixeles);

    int minX = textura.Width, minY = textura.Height, maxX = 0, maxY = 0;

    for (int y = 0; y < textura.Height; y++)
    {
        for (int x = 0; x < textura.Width; x++)
        {
            if (pixeles[y * textura.Width + x].A > 10)
            {
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }
        }
    }

    return new Rectangle(minX, minY, maxX - minX + 1, maxY - minY + 1);
}

    private Panel _menuPanel;

    private void CrearMenu()
    {
        _menuPanel = new Panel();
        _menuPanel.Dock(Gum.Wireframe.Dock.Fill);
        _menuPanel.AddToRoot();

        var botonJugar = new Button();
        botonJugar.Anchor(Gum.Wireframe.Anchor.Center);
        botonJugar.Y = 150;
        botonJugar.Width = 100;
        botonJugar.Text = "Jugar";
        botonJugar.Click += (sender, args) => _menuPanel.IsVisible = false;
        _menuPanel.AddChild(botonJugar);

        var botonSalir = new Button();
        botonSalir.Anchor(Gum.Wireframe.Anchor.Center);
        botonSalir.Y = 190;
        botonSalir.Width = 100;
        botonSalir.Text = "Salir";
        botonSalir.Click += (sender, args) => Exit();
        _menuPanel.AddChild(botonSalir);

        botonJugar.IsFocused = true;
    }
    protected override void Update(GameTime gameTime)
      
    {   GumService.Default.Update(gameTime);

        // Mientras el menú esté visible, no se mueven los personajes
        if (_menuPanel.IsVisible)
         return;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
       KeyboardState teclado = Keyboard.GetState();

    caminando1 = false;

    if (teclado.IsKeyDown(Keys.Right))
    {
    posicion.X += 3;
    caminando1 = true;
    efecto1 = SpriteEffects.None;
    }

    if (teclado.IsKeyDown(Keys.Left))
    {
        posicion.X -= 3;
    caminando1 = true;
    efecto1 = SpriteEffects.FlipHorizontally;
    }

    if (teclado.IsKeyDown(Keys.D))
    {
        posicion2.X += 3;
    }

    if (teclado.IsKeyDown(Keys.A))
    {
        posicion2.X -= 3;
    }
       posicion.X = MathHelper.Clamp( 
         posicion.X,
         0,
          GraphicsDevice.Viewport.Width - personaje.Width * escala1
);

        posicion2.X = MathHelper.Clamp(
        posicion2.X,
        0,
         GraphicsDevice.Viewport.Width - personaje2.Width * escala2
);
        
  
        if (caminando1)
{
    temporizador1 += gameTime.ElapsedGameTime.TotalSeconds;
    if (temporizador1 >= tiempoPorFrame)
    {
        temporizador1 = 0;
        frameActual1 = (frameActual1 + 1) % totalFrames;
    }
}
else
{
    frameActual1 = 0;
    temporizador1 = 0;
}
    base.Update(gameTime);
    }

   protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.MonoGameOrange);

    if (_menuPanel.IsVisible)
    {
        // Solo el fondo del menú
        _spriteBatch.Begin();
        _spriteBatch.Draw(
            _fondoInterfaz,
            new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
            Color.White);
        _spriteBatch.End();
    }
    else
{
    // Fondo del juego + personajes
    _spriteBatch.Begin();
    _spriteBatch.Draw(
        fondo,
        new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
        Color.White);

    // Personaje 1 (animado)
    Rectangle origen = new Rectangle(frameActual1 * frameAncho, 0, frameAncho, frameAlto);
    _spriteBatch.Draw(personaje, posicion, origen, Color.White, 0f, Vector2.Zero, escala1, efecto1, 0f);

    // Personaje 2 (sigue igual por ahora)
    _spriteBatch.Draw(personaje2, posicion2, null, Color.White, 0f, Vector2.Zero, escala2, SpriteEffects.None, 0f);

    _spriteBatch.End();
}

    GumService.Default.Draw();
    base.Draw(gameTime);
}
}