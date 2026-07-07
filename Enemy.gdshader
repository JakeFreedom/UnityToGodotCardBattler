shader_type canvas_item;

uniform vec4 flash_color: source_color= vec4(1.0);
uniform float flash_pct: hint_range(0.0, 1.0) = 0.0;

void fragment() {
	// Called for every pixel the material is visible on.
	vec4 originalColor = texture(TEXTURE, UV);
	COLOR = mix(originalColor, flash_color, flash_pct);
	COLOR.a *= originalColor.a;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
