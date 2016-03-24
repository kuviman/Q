uniform sampler2D texture;
uniform sampler2D alphaTexture;
uniform int chunkSize;
uniform vec2 chunk;

varying vec3 modelPos;

void main() {
	gl_FragColor = texture2D(texture, modelPos.xy) * texture2D(alphaTexture, (modelPos.xy + vec2(0.5, 0.5) - chunk * chunkSize) / float(chunkSize + 1));
}