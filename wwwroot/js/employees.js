// Fill form for editing
function fillForm(id,ten,machucvu,macongty){
    document.getElementById('id').value = id;
    document.getElementById('ten').value = ten;
    document.getElementById('machucvu').value = machucvu;
    document.getElementById('macongty').value = macongty;
}

// Quick select all item
document.getElementById('selectAll').addEventListener('change',function(){
    const checkboxes = document.querySelectorAll('.employee-checkbox');
    checkboxes.forEach(checkbox => {
        checkbox.checked = this.checked;
    });
    updateDeleteButton();
});

// Update button state
function updateDeleteButton(){
    const checkboxes =document.querySelectorAll('.employee-checkbox:checked');
    const deleteButton = document.getElementById('deleteSelectedBtn');
    deleteButton.disabled = checkboxes.length === 0;
}

document.querySelectorAll('.employee-checkbox').forEach(checkbox => {
    checkbox.addEventListener('change',updateDeleteButton);
});


// confirm toast
// delete multi button
document.getElementById('deleteSelectedBtn').addEventListener('click', function() {
    event.preventDefault(); // Stop default submission

    if (confirm('Are you sure you want to delete the selected employees?')) {
        document.getElementById('editForm').action = '?handler=DeleteMulti'; 
        const checkboxElements = document.querySelectorAll('.employee-checkbox:checked')
        const selectedIds = Array.from(checkboxElements).map(checkbox =>checkbox.value);
        selectedIds.forEach(id => {
            const input =document.createElement('input');
            input.type ='hidden'
            input.name ='selectedIds'
            input.value =id;
            document.getElementById('editForm').appendChild(input);
        });
        document.getElementById('editForm').submit();        
}})

// delete button 
document.getElementById('deleteForm').addEventListener('click', function(event) {
    if (confirm('Are you sure you want to delete the selected employee')) {
        document.getElementById('deleteForm').submit();
    }
});

// edit button on Row 
document.getElementById('editButton').addEventListener('click', function(event) {
    if (confirm('Are you sure you want to update this employee?')) {
        document.getElementById('editForm').action = '?handler=Update';
        document.getElementById('editForm').submit();
    }
});
