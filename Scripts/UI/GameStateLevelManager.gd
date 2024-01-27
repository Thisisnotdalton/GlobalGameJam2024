extends Node

func _on_Game_OnStateChanged(enabled):
	if enabled:
		$LevelLoader.call_deferred("LoadDefaultLevel")
	else:
		$LevelLoader.call_deferred("UnloadCurrentLevel")
