<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

  <xsl:template match="dot_dep_customerinsmember_ag">
    <table class="tblDesc">
      <tr>
        <td class="colW1">{lbl:DisableOpenTote}</td>
        <td class="colW2">
          <xsl:value-of select="@disableopentote" />
        </td>

        <td class="colW1">{lbl:DisableHorseRacing}</td>
        <td class="colW2">
          <xsl:value-of select="@disablehorseracing" />
        </td>
        <td class="colW1">{lbl:DisableFinancial}</td>
        <td class="colW2">
          <xsl:value-of select="@disablefinancial" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:suspended}</td>
        <td class="colW2">
          <xsl:value-of select="@suspended" />
        </td>

        <td class="colW1"></td>
        <td class="colW2">

        </td>
        <td class="colW1"></td>
        <td class="colW2">

        </td>
      </tr>
    </table>
  </xsl:template>


</xsl:stylesheet>
