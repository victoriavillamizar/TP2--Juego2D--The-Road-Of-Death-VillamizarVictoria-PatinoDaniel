using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
       
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    
        string ruta = Path.Combine(
        AppContext.BaseDirectory,
        "content",
        "personaje.png"
        );
    
        using (FileStream archivo = File.OpenRead(ruta))
        {
            personaje = Texture2D.FromStream(GraphicsDevice, archivo);
        }

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
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        KeyboardState teclado = Keyboard.GetState();
        if (teclado.IsKeyDown(Keys.Right))
        {
            posicion.X += 3;
        }
         if (teclado.IsKeyDown(Keys.D))
            posicion2.X += 3;
        if (teclado.IsKeyDown(Keys.A))
            posicion2.X -= 3;

        if (teclado.IsKeyDown(Keys.Left))
        {
            posicion.X -= 3;
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
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MonoGameOrange);
        _spriteBatch.Begin();
         _spriteBatch.Draw(
        fondo,
        new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
        Color.White
         );
        _spriteBatch.Draw(personaje, posicion, null, Color.White, 0f, Vector2.Zero, escala1, SpriteEffects.None, 0f);
        _spriteBatch.Draw(personaje2, posicion2, null, Color.White, 0f, Vector2.Zero, escala2, SpriteEffects.None, 0f);
        _spriteBatch. End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}