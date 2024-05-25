using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    private static Controls controls;
    private static Vector3 mousePos;

    public static Vector3 GetMousePos()
    {
        return mousePos;
    }
   public static void Init(Player player)
    {
        controls = new Controls();

        controls.InGame.Movement.performed += _ =>
        {
            player.SetMoveDirection(_.ReadValue<Vector2>());
        };

        controls.InGame.Shoot.performed += _ =>
        {
            player.Shoot();
        };
        controls.InGame.Reload.performed += _ =>
        {
            player.Reload();
        };
        controls.InGame.MousePos.performed += _ =>
        {
            mousePos = _.ReadValue<Vector2>();
        };
        controls.InGame.Swap.performed += _ =>
        {
            player.switchWeapon(_.ReadValue<float>());
        };
       

    }

    public static void EnableInGame()
    {
        controls.InGame.Enable();
    }
    public static void DisableInGame()
    {
        controls.InGame.Disable();
    }
}
