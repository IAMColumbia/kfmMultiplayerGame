## 02/17/2026
I was originally going for something like SpyParty, but after feedback during class, I decided to change direction.
<br><br>
I tried to think about PvP games I've played & enjoyed. Unfortunately, I haven't played many PvP games & the ones I've enjoyed all have an element of social deduction, which means you can't see the other player's screen.
(Examples: SpyParty, Among Us, Dead by Daylight)
<br><br>
I decided to start building a map & cultivate an atmosphere as I continued thinking about what direction to take it. I love games with a heavy atmosphere & it's not unheard of for atmosphere-driven games start with the world/environment before deciding on the mechanics.
My map/theme is inspired by dead malls, vaporwave, & the type of liminal spaces you might find in PS1-era games (especially open-world games).
## 02/24/2026
As I created the world space, I kept thinking about GoldenEye, which is the one PvP multiplayer game I've played (& liked) that isn't a social deduction game. GoldenEye also fits that low-poly aesthetic I'm going for. However, I didn't want to completely rip off GoldenEye.
<br><br>
Ultimately, I landed on a game that's a cross between GoldenEye and Dead by Daylight, where one player is a survivor & the other player is a zombie. (I felt like zombies = a play on "dead" malls.)
Since the environment has a lot of repeating elements, it won't immediately give away where the other player is, even if you can see their screen.
## 03/03/2026
I added the camera scripting and player prefabs for the human and the zombie. I've decided to balance it by making the zombie more tanky and the human more squishy.
## 03/05/2026
I went back to the drawing board. I put a lot of effort into cultivating a surreal, liminal atmosphere in my game, & I feel like making it a deathmatch/FPS-style game detracts from that. Instead, I've reverted to the idea of social deduction, which (in my opinion) lends itself better to the vibes of my game.
<br><br>
I researched couch multiplayer games with social deduction elements & found a few. For the most part, they seem to be top-down third-person games, which won't work with my current map, but I think this could add to the fun with the right ruleset.
<br><br>
My idea: It's like Where's Waldo + Hide & Seek. Each player appears as a random object on the screen, and all players must explore the environment to find the other competitors. If a player thinks they've found one of their competitors, they must attempt to "tag" the object. When a player is successfully tagged, they respawn as a new object. On an unsuccessful tag (i.e., a player clicks on an inanimate object, misses the other player, or is too far away), the player who made the attempt is reset to a spawn point (prevents tag spamming). The player with the most successful tags wins.
<br><br>
Players can turn the tide in their favor by finding various pickups. Pickup effects include:
<br>
- changing the player's appearance (e.g., a player who spawns in as a plant might change into a statue)
- invisibility (temporary)
- teleport
- speed increase (temporary)
<br><br>

Like I mentioned above, the first-person POV + shared screen creates a dilemma that doesn't exist in typical social deduction games like Among Us. Players can look at each other's screens & reasonably guess where the other player is located. However, the repeated elements in the environment add a challenge, & even if a player figures out where another player is, item pickups give competitors a chance to evade each other.
<br><br>
Another strategy is to use the environment. Players can try to blend in (e.g., a player who appears as a plant could hide in one of the many planters around the map; a player who appears as a soda cup could hide in the food court, etc.) or run away & use their knowledge of the map to lose a competitor who's chasing them.
