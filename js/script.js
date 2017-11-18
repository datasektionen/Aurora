window.addEventListener('DOMContentLoaded', function() {
	const inputs = document.querySelectorAll('input[type="file"]')
	console.log(inputs)	

	Array.prototype.forEach.call(inputs, function(input) {
	const label = input.nextElementSibling;
	const labelVal = label.innerHTML;

	input.addEventListener('change', function(e) {
		let fileName = '';
		if (this.files && this.files.length > 1) {
			fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
		} else {
			fileName = e.target.value.split('\\').pop();
		}

		if (fileName) {
			label.querySelector('span.val').innerHTML = fileName;
		} else {
			label.innerHTML = labelVal;
		}
	});
});
}, true);