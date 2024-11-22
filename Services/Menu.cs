using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services
{
    public class Menu
    {
        private readonly GameContext _gameContext;

        public Menu(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("1. Display Rooms");
                Console.WriteLine("2. Display Characters");
                Console.WriteLine("3. Add Room");
                Console.WriteLine("4. Add Character");
                Console.WriteLine("5. Find Character");
                Console.WriteLine("6. Remove Character");
                Console.WriteLine("7. Remove Room");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayRooms();
                        break;
                    case "2":
                        DisplayCharacters();
                        break;
                    case "3":
                        AddRoom();
                        break;
                    case "4":
                        AddCharacter();
                        break;
                    case "5":
                        FindCharacter();
                        break;
                    case "6":
                        RemoveCharacter();
                        break;
                    case "7":
                        RemoveRoom();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        public void DisplayRooms()
        {
            var rooms = _gameContext.Rooms.Include(r => r.Characters).ToList();

            foreach (var room in rooms)
            {
                Console.WriteLine($"Room: {room.Name} - {room.Description}");
                foreach (var character in room.Characters)
                {
                    Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
                }
            }
        }

        public void DisplayCharacters()
        {
            var characters = _gameContext.Characters.ToList();
            if (characters.Any())
            {
                Console.WriteLine("\nCharacters:");
                foreach (var character in characters)
                {
                    Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
                }
            }
            else
            {
                Console.WriteLine("No characters available.");
            }
        }

        public void AddRoom()
        {
            Console.Write("Enter room name: ");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Room name cannot be empty.");
                return;
            }

            Console.Write("Enter room description: ");
            var description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
            {
                Console.WriteLine("Room description cannot be empty.");
                return;
            }

            var room = new Room
            {
                Name = name,
                Description = description
            };

            _gameContext.Rooms.Add(room);
            _gameContext.SaveChanges();

            Console.WriteLine($"Room '{name}' added to the game.");
        }

        public void AddCharacter()
        {
            Console.Write("Enter character name: ");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Character name cannot be empty.");
                return;
            }

            Console.Write("Enter character level: ");
            if (!int.TryParse(Console.ReadLine(), out var level))
            {
                Console.WriteLine("Invalid level. Please enter a valid number.");
                return;
            }

            Console.Write("Enter room ID for the character: ");
            if (!int.TryParse(Console.ReadLine(), out var roomId))
            {
                Console.WriteLine("Invalid room ID. Please enter a valid number.");
                return;
            }

            // Find the room by ID
            var room = _gameContext.Rooms.Find(roomId);
            if (room == null)
            {
                Console.WriteLine($"Room with ID {roomId} does not exist.");
                return;
            }

            // Create a new character and add it to the room
            var character = new Character
            {
                Name = name,
                Level = level,
                RoomId = roomId,
                Room = room,
                Abilities = new List<Ability>()
            };

            _gameContext.Characters.Add(character);
            _gameContext.SaveChanges();

            Console.WriteLine($"Character '{name}' added to room '{room.Name}'.");
        }

        public void FindCharacter()
        {
            Console.Write("Enter character name to search: ");
            var name = Console.ReadLine();

            // Use LINQ to query the database for the character
            var character = _gameContext.Characters
                                        .Include(c => c.Room)
                                        .FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

            if (character != null)
            {
                Console.WriteLine($"Character found: ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room: {character.Room.Name}");
            }
            else
            {
                Console.WriteLine($"Character with name '{name}' not found.");
            }
        }

        public void RemoveCharacter()
        {
            Console.Write("Enter character name to remove: ");
            var name = Console.ReadLine();

            // Use LINQ to query the database for the character
            var character = _gameContext.Characters
                                        .Include(c => c.Room)
                                        .FirstOrDefault(c => c.Name.ToLower() == name.ToLower());

            if (character != null)
            {
                _gameContext.Characters.Remove(character);
                _gameContext.SaveChanges();
                Console.WriteLine($"Character '{name}' removed from the game.");
            }
            else
            {
                Console.WriteLine($"Character with name '{name}' not found.");
            }
        }

        public void RemoveRoom()
        {
            Console.Write("Enter room name to remove: ");
            var name = Console.ReadLine();

            // Use LINQ to query the database for the room
            var room = _gameContext.Rooms
                                   .Include(r => r.Characters)
                                   .FirstOrDefault(r => r.Name.ToLower() == name.ToLower());

            if (room != null)
            {
                if (room.Characters.Any())
                {
                    Console.WriteLine($"Room '{name}' cannot be removed because it contains characters.");
                }
                else
                {
                    _gameContext.Rooms.Remove(room);
                    _gameContext.SaveChanges();
                    Console.WriteLine($"Room '{name}' removed from the game.");
                }
            }
            else
            {
                Console.WriteLine($"Room with name '{name}' not found.");
            }
        }
    }
}
