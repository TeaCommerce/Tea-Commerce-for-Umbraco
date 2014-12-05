jQuery(function () {
  jQuery('.pnlCurrencies').toggleClass('hide');

  if (!jQuery('.specialSymbol input').attr("checked")) {
    jQuery('#specialSymbolContainer').toggle();
  }

  jQuery('.specialSymbol').click(function () {
    jQuery('#specialSymbolContainer').toggle();
  });

  jQuery('.customPrices a').click(function (e) {
    var jQEle = jQuery(this);
    jQEle.closest('div').next('div.pnlCurrencies').toggleClass('hide');
    return false;
  });

  checkSelectAllStatus();

  jQuery('.selectAll').click(function () {
    var selectAll = jQuery(this),
        selectAllInput = selectAll.find('input[type=checkbox]'),
        allInputs = selectAll.next().find('input[type=checkbox][disabled!=disabled]'),
        checked = selectAllInput.is(':checked');
    
    allInputs.attr('checked', checked);
  });

  jQuery('.checkboxesForSelectAll input[type=checkbox][disabled!=disabled]').click(function () {
    checkSelectAllStatus();
  });
});

function checkSelectAllStatus() {
  jQuery('.checkboxesForSelectAll').each(function () {
    var checkboxesForSelectAll = jQuery(this),
        selectAll = checkboxesForSelectAll.prev(),
        selectAllInput = selectAll.find('input[type=checkbox]'),
        allInputsDisabled = checkboxesForSelectAll.find('input[type=checkbox]:not(:checked)'),
        checked = allInputsDisabled.length == 0;

    selectAllInput.attr('checked', checked);
  });
}


