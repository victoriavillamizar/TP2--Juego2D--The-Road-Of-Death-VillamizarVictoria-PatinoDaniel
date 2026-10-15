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
    private const float escala = 0.3f;
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
            GraphicsDevice.Viewport.Width - personaje.Width * escala
        );

        posicion2.X = MathHelper.Clamp(
            posicion2.X,
            0,
            GraphicsDevice.Viewport.Width - personaje2.Width * escala
        );
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MonoGameOrange);
        _spriteBatch.Begin();
        _spriteBatch.Draw(personaje,posicion,null,Color.White,0f,Vector2.Zero,0.3f,SpriteEffects.None,0f);
        _spriteBatch.Draw(personaje2, posicion2, null, Color.White, 0f, Vector2.Zero, escala, SpriteEffects.None, 0f);
        _spriteBatch. End();
        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}